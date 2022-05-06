using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using ForesterNodeCore;
using ForresterModeller.Pages.Tools;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Nodes.Viters;
using ForresterModeller.src.ProjectManager;
using ForresterModeller.src.ProjectManager.WorkArea;
using NodeNetwork.Views;
using ReactiveUI;
using WpfMath.Controls;

namespace ForresterModeller.src.Windows.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public TabControlViewModel TabControlVM { get; } = new();
        public PropertiesControlViewModel PropertiesVM { get; set; } = new();
        public ObservableCollection<FormulaControl> Formulas { get; set; }
        public PlotterTools PlotterToolsVM { get; set; } = new();
        public DiagramTools DiagramToolsVM { get; set; }
        public ContentControl ToolContent { get; set; } = new();
        public static Project ProjectInstance;
        private Project _activeProject;
        public Project ActiveProject
        {
            get => _activeProject;
            set
            {
                this.RaiseAndSetIfChanged(ref _activeProject, value);
                ProjectInstance = _activeProject;
            }
        }
        private StartWindowViewModel _startWindowVM { get; set; }

        #region commands
        //Создать новую диаграмму
        public ReactiveCommand<WorkAreaManager, Unit> CreateDiagramTab { get; }
        /// <summary>
        /// Открыть элемент по имени
        /// </summary>
        public ReactiveCommand<String, Unit> OpenTab { get; }
        public ReactiveCommand<TabViewModel, Unit> CloseTab { get; }
        public ReactiveCommand<Unit, Unit> CalculateByCore { get; }
        public ReactiveCommand<Unit, Unit> OpenTestGraph { get; }
        public ReactiveCommand<Unit, Unit> InitProjectByPath { get; }
        public ReactiveCommand<Unit, Unit> CreateNewProject { get; }
        public ReactiveCommand<Unit, Unit> OpenMathView { get; }
        public ReactiveCommand<Unit, Unit> SaveProject { get; }
        public ReactiveCommand<Unit, Unit> CloseAllTab { get; }

        #endregion
        public MainWindowViewModel(Project project, StartWindowViewModel StartWindowVM)
        {
            _startWindowVM = StartWindowVM;
            ActiveProject = project;
            ActiveProject.PropertyChanged += ActiveProject_PropertyChanged;
            TabControlVM.PropertyChanged += ActiveTabChanged;
            CreateDiagramTab = ReactiveCommand.Create<WorkAreaManager>(o => AddTab(CreateDiagramManager()));
            OpenTab = ReactiveCommand.Create<String>(s => AddTab(new DiagramManager(ActiveProject) { Name = "Диаграмма12" }));
            CloseTab = ReactiveCommand.Create<TabViewModel>(o => TabControlVM.Tabs.Remove(o));
            CalculateByCore = ReactiveCommand.Create<Unit>(o => ExecuteModelling());
            InitProjectByPath = ReactiveCommand.Create<Unit>(u => InitiateProjectByPath());
            CreateNewProject = ReactiveCommand.Create<Unit>(u => CreateProject());
            OpenMathView = ReactiveCommand.Create<Unit>(o => AddMathView());
            DiagramToolsVM = new(project: ActiveProject);
            SaveProject = ReactiveCommand.Create<Unit>(u => SaveProj());
            CloseAllTab = ReactiveCommand.Create<Unit>(u => CloseAllTabs());
        }

        private void ActiveProject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var proj = (Project)sender;
        }

        /// <summary>
        /// ДДОБАВЛЯТЬ ВКЛАДКИ ТОЛЬКО ЧЕРЕЗ ЭТОТ МЕТОД!!!
        /// иначе окно пропертей не будет обновляться
        /// </summary>
        /// <param name="contentManager"></param>
        private void AddTab(WorkAreaManager contentManager)
        {
            if (contentManager != null)
            {
                contentManager.PropertySelectedEvent += sender => PropertiesVM.ActiveItem = sender;
                TabControlVM.AddTabFromWAManager(contentManager);
            }
        }
        public void CloseAllTabs()
        {
            TabControlVM.Tabs.Clear();
            TabControlVM.ActiveTab = null;
        }
        public void OpenOrCreateTab(WorkAreaManager contentManager)
        {
            foreach (var tab in TabControlVM.Tabs)
            {
                if (tab.WAManager == contentManager)
                {
                    TabControlVM.ActiveTab = tab;
                    return;
                }
            }
            AddTab(contentManager);
        }
        public DiagramManager CreateDiagramManager()
        {
            var diagramManager = new DiagramManager(ActiveProject);
            diagramManager.Name = "диаграмма 2123у1";
            ActiveProject.AddDiagram(diagramManager);
            return diagramManager;
        }
        private void ActiveTabChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(TabControlViewModel.ActiveTab))
            {
                var tbControl = ((TabControlViewModel)sender);
                var manager = tbControl.ActiveTab?.WAManager;
                if (manager is DiagramManager)
                {
                    ToolContent.Content = DiagramToolsVM;
                }
                else if (manager is PlotManager)
                {
                    PlotterToolsVM.DataContext = manager;
                    ToolContent.Content = PlotterToolsVM;
                }
            }
        }

        private void ExecuteModelling()
        {
            {
                try
                {
                    AddTab(CalculateGraphByCore());
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show("Ваша модель некорректна!\n" + e.Message);
                }
            }
        }
        private void AddMathView()
        {

            if (TabControlVM.ActiveTab == null || TabControlVM.ActiveTab.WAManager is not DiagramManager)
            {
                System.Windows.MessageBox.Show("Откройте диаграмму, по которой необходимо построить мат. модель");
            }
            else
            {
                var diagr = (DiagramManager)TabControlVM.ActiveTab.WAManager;
                var math = new MatViewManager(diagr);
                math.PropertySelectedEvent += sender => PropertiesVM.ActiveItem = sender;
                AddTab(math);

            }

        }
        private PlotManager CalculateGraphByCore()
        {
            if (TabControlVM.ActiveTab?.WAManager is not DiagramManager)
                return null;
            var manager = (DiagramManager)TabControlVM.ActiveTab.WAManager;
            double t = manager.AllTime, dt = manager.DeltaTime;
            var network = ((NetworkView)manager.Content).ViewModel;
            string text = NodeTranslator.Translate(network);
            List<NodeIdentificator> ids = new();
            foreach (var nod in network.Nodes.Items)
            {
                if (nod is not CrossNodeModel && nod is not LinkNodeModel)
                    ids.Add(new NodeIdentificator(((ForesterNodeModel)nod).Id));
            }
            var c = ForesterNodeCore.Program.GetCurve(text,
               ids,
                t,
                dt
            );
            PlotManager plotmodel = new(c, t, dt, ActiveProject);
            plotmodel.XLabel = "Время (недели)FFF";
            plotmodel.YLabel = "Объем товара (единицы) ";
            plotmodel.Name = "График123";
            return plotmodel;
        }

        /// <summary>
        /// Открыть существующий проект
        /// Открывает диаологовое окно, по выбранному json инициализирует активный проект необходимыми данными
        /// </summary>
        private void InitiateProjectByPath()
        {

            if (ActiveProject != null)
            {
                ActiveProject.SaveOldProject(_startWindowVM);
                CloseAllTabs();
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы json|*.json";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ActiveProject = Loader.InitProjectByPath(openFileDialog.FileName);
                _startWindowVM.AddProject(ActiveProject.GetFullName());
                //ИЗМЕНИТЬ СОДЕРЖИМОЕ ОКНА ЕЩЕ
            }
        }

        /// <summary>
        /// Создать проект
        /// создает и инициализирует активный проект
        /// </summary>
        private void CreateProject()
        {
            if (ActiveProject != null)
            {
                ActiveProject.SaveOldProject(_startWindowVM);
            }
            CreateProject proj = new CreateProject();
            var window = proj as Window;
            var dialogResult = window.ShowDialog();

            if (dialogResult == true)
            {
                ActiveProject = Loader.InitProjectByPath(proj.ViewModel.FileName);
                _startWindowVM.AddProject(proj.ViewModel.FileName);
                //ИЗМЕНИТЬ СОДЕРЖИМОЕ ОКНА ЕЩЕ
            }
        }

        /// <summary>
        /// Создать проект
        /// создает и инициализирует активный проект
        /// </summary>
        private void SaveProj()
        {
            if (ActiveProject != null)
            {
                ActiveProject.SaveOldProject(_startWindowVM);
            }

        }
    }
}