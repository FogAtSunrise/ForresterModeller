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
using ForresterModeller.ProjectManager.WorkArea;
using ForresterModeller.src.ProjectManager;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Nodes.Viters;
using ForresterModeller.src.Pages.Tools;
using ForresterModeller.src.ProjectManager.WorkArea;
using NodeNetwork.Views;
using ReactiveUI;
using WpfMath.Controls;
using ForresterModeller.src.Windows;

namespace ForresterModeller.Windows.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public TabControlViewModel TabControlVM { get; } = new();
        public PropertiesControlViewModel PropertiesVM { get; set; } = new();
        public ObservableCollection<FormulaControl> Formulas { get; set; }
        public PlotterTools PlotterToolsMW { get; set; } = new();
        public DiagramTools DiagramToolsWM { get; set; } = new();
        public ContentControl ToolContent { get; set; } = new();
        public Project ActiveProject { get; set; }


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

        #endregion
        public MainWindowViewModel(Project project)
        {
            ActiveProject = project;
            TabControlVM.PropertyChanged += ActiveTabChanged;
            CreateDiagramTab = ReactiveCommand.Create<WorkAreaManager>(o => AddTab(CreateDiagramManager()));
            OpenTab = ReactiveCommand.Create<String>(s => AddTab(new DiagramManager { Name = "Диаграмма12" }));
            CloseTab = ReactiveCommand.Create<TabViewModel>(o => TabControlVM.Tabs.Remove(o));
            CalculateByCore = ReactiveCommand.Create<Unit>(o => ExecuteModelling());
            OpenTestGraph = ReactiveCommand.Create<Unit>(u => AddTab(TestPlot()));
            InitProjectByPath = ReactiveCommand.Create<Unit>(u => InitiateProjectByPath());
            CreateNewProject = ReactiveCommand.Create<Unit>(u => CreateProject());
            OpenMathView = ReactiveCommand.Create<Unit>(o => AddMathView());

            SaveProject = ReactiveCommand.Create<Unit>(u => SaveProj());
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

        public DiagramManager CreateDiagramManager()
        {
            var diagramManager = new DiagramManager();
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
                    ToolContent.Content = DiagramToolsWM;
                }
                else if (manager is PlotManager)
                {
                    ToolContent.Content = PlotterToolsMW;
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
                catch
                {
                    System.Windows.MessageBox.Show("Ваша модель некорректна!");
                }
            }
        }
        private void AddMathView()
        {
            if (TabControlVM.ActiveTab.WAManager is not DiagramManager)
            {
                System.Windows.MessageBox.Show("Откройте диаграмму, по которой необходимо построить мат. модель");
            }
            else
            {

                
                var math = new MatViewManager { Name = "MathView_For_" + TabControlVM.ActiveTab.Header };
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
                if (nod is not CrossNodeModel)
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

        private PlotManager TestPlot()
        {
            PlotManager plotmodel = new();
            plotmodel.Lines.Add(new PlotManager.Line(new double[] { 1, 2, 3, 4 }, new double[] { 1, 2, 3, 4 }, "Продуктивность студента"));
            plotmodel.XLabel = "Степень окончания семестра";
            plotmodel.YLabel = "Скорость сдачи лаб ";
            plotmodel.Name = "График продуктивности";
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
                ActiveProject.SaveOldProject();
                System.Windows.MessageBox.Show("Сохранился текущий активный проект " + ActiveProject.getName()); ////////////////////////////////
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы json|*.json";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ActiveProject = Loader.InitProjectByPath(openFileDialog.FileName);
                if (ActiveProject != null)
                    System.Windows.MessageBox.Show("Открылся " + ActiveProject.getName());////////////////////////////////

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
                ActiveProject.SaveOldProject();
                System.Windows.MessageBox.Show("Сохранился текущий активный проект " + ActiveProject.getName());////////////////////////////////
            }
            CreateProject proj = new CreateProject();
            var window = proj as Window;
            var dialogResult = window.ShowDialog();

            if (dialogResult == true)

            {
                //System.Windows.MessageBox.Show("Открылсяzzzzzzzzzzzzz " + proj.ViewModel.FileName);
                ActiveProject = Loader.InitProjectByPath(proj.ViewModel.FileName);
                if (ActiveProject != null)
                    System.Windows.MessageBox.Show("Открылся " + ActiveProject.getName());////////////////////////////////

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
                ActiveProject.SaveOldProject();
                System.Windows.MessageBox.Show("Сохранился текущий активный проект " + ActiveProject.getName());////////////////////////////////
            }

        }
        public ContentControl ExecuteCore()
        {
            float t = 1, dt = 0.1f;
            var c = ForesterNodeCore.Program.GetCurve(
                "c a 1 | c b 2 | l dc b a 0 | f nt dc/2 | d boo loo 1 b nt 0",
                new List<NodeIdentificator> {
                    new NodeIdentificator("dc"),
                    new NodeIdentificator("nt"),
                    new NodeIdentificator("boo"),
                    new NodeIdentificator("loo"),
                },
                t,
                dt
            );
            var plotmodel = new PlotManager(c, t, dt, ActiveProject);
            plotmodel.XLabel = "Время (недели)FFF";
            plotmodel.YLabel = "Объем товара (единицы) ";
            return plotmodel.GenerateActualPlot();
        }

    }
}