using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Resources;
using System.Windows.Xps.Packaging;
using DynamicData;
using ForesterNodeCore;
using ForresterModeller.Pages.Tools;
using ForresterModeller.src.Interfaces;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Nodes.Viters;
using ForresterModeller.src.ProjectManager;
using ForresterModeller.src.ProjectManager.WorkArea;
using ForresterModeller.src.Windows.Views;
using NodeNetwork.Views;
using ReactiveUI;
using WpfMath.Controls;

namespace ForresterModeller.src.Windows.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public ObservableCollection<Project> ListFromProject { get; private set; }
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
                ListFromProject = new ObservableCollection<Project>() { ActiveProject };
                ProjectInstance = _activeProject;
            }
        }
        private StartWindowViewModel _startWindowVM { get; set; }


        private FixedDocumentSequence _doc { get; set; } = null;

        #region commands
        //Создать новую диаграмму
        public ReactiveCommand<WorkAreaManager, Unit> CreateDiagramTab { get; }
        /// <summary>
        /// Открыть элемент по имени
        /// </summary>
        public ReactiveCommand<String, Unit> OpenTabCommand { get; }
        public ReactiveCommand<TabViewModel, Unit> CloseTab { get; }
        public ReactiveCommand<Unit, Unit> CalculateByCore { get; }
        public ReactiveCommand<Unit, Unit> InitProjectByPath { get; }
        public ReactiveCommand<Unit, Unit> CreateNewProject { get; }
        public ReactiveCommand<Unit, Unit> OpenMathViewCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveProject { get; }
        public ReactiveCommand<Unit, Unit> DeleteProjectCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveAsProjectCommand { get; }
        public ReactiveCommand<Unit, Unit> CloseAllTab { get; }
        public ReactiveCommand<IPropertyOwner, Unit> OpenPropertyCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateTab { get; }


        public ReactiveCommand<Unit, Unit> ShowHelpWindow{get;}

        public ReactiveCommand<Unit, Unit> ShowAftorWindow{get;}
        public Package ResourceManager { get; private set; }



        #endregion
        public MainWindowViewModel(Project project, StartWindowViewModel StartWindowVM)
        {
            _startWindowVM = StartWindowVM;
            ActiveProject = project;
            ActiveProject.PropertyChanged += ActiveProject_PropertyChanged;
            TabControlVM.PropertyChanged += ActiveTabChanged;
            CreateDiagramTab = ReactiveCommand.Create<WorkAreaManager>(o => OpenTab(CreateDiagramManager()));
            OpenTabCommand = ReactiveCommand.Create<String>(s => OpenTab(new DiagramManager(ActiveProject) { Name = "Диаграмма12" }));
            CloseTab = ReactiveCommand.Create<TabViewModel>(o => TabControlVM.Tabs.Remove(o));
            CalculateByCore = ReactiveCommand.Create<Unit>(o => ExecuteModelling());
            InitProjectByPath = ReactiveCommand.Create<Unit>(u => InitiateProjectByPath());
            CreateNewProject = ReactiveCommand.Create<Unit>(u => CreateProject());
            OpenMathViewCommand = ReactiveCommand.Create<Unit>(o => OpenMathView());
            DiagramToolsVM = new(project: ActiveProject);
            SaveProject = ReactiveCommand.Create<Unit>(u => SaveProj());
            DeleteProjectCommand = ReactiveCommand.Create<Unit>(u => DeleteProject());
            SaveAsProjectCommand = ReactiveCommand.Create<Unit>(u => SaveAsProject());
            CloseAllTab = ReactiveCommand.Create<Unit>(u => CloseAllTabs());
            OpenPropertyCommand = ReactiveCommand.Create<IPropertyOwner>(u =>
            {
                if (u != null)
                    PropertiesVM.ActiveItem = u;
            });

            ShowHelpWindow = ReactiveCommand.Create<Unit>(u =>
                {

                    if (_doc is null)
                    {
                        Stream stream = new MemoryStream(Resource.Doc);
                        var package = Package.Open(stream);
                        string uri = "memorystream://myXps.xps";
                        Uri packageUri = new Uri(uri);
                        PackageStore.AddPackage(packageUri, package);
                        XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.Maximum, uri);
                        _doc = xpsDocument.GetFixedDocumentSequence();
                    }
                    new HelpWindow(_doc).Show();
                }
            );
            ShowAftorWindow = ReactiveCommand.Create<Unit>(u =>
                ExecuteModelling()
            );

            UpdateTab = ReactiveCommand.Create<Unit>(u => UpdateActiveTab());
        }

        private void ActiveProject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var proj = (Project)sender;
        } 
        private void DeleteProject()
        {
           ActiveProject.RemoveFromFileSystem();
           ActiveProject = null;
        }

        /// <summary>
        /// ДДОБАВЛЯТЬ ВКЛАДКИ ТОЛЬКО ЧЕРЕЗ ЭТОТ МЕТОД!!!
        /// иначе окно пропертей не будет обновляться
        /// </summary>
        /// <param name="contentManager"></param>
        private TabViewModel OpenTab(WorkAreaManager contentManager)
        {
            if (contentManager != null)
            {
                contentManager.PropertySelectedEvent += sender => PropertiesVM.ActiveItem = sender;
                return TabControlVM.AddTabFromWAManager(contentManager);
            }
            return null;
        }
        public void CloseAllTabs()
        {
            TabControlVM.Tabs.Clear();
            TabControlVM.ActiveTab = null;
        }
        public void UpdateActiveTab()
        {
            if (TabControlVM.ActiveTab == null)
            {
                System.Windows.MessageBox.Show("Откройте элемент для обновления");
            }
            else
            {
                TabViewModel tab = TabControlVM.ActiveTab;
                TabControlVM.ActiveTab = null;
                TabControlVM.ActiveTab = tab;
            }
        }
        public void OpenOrCreateTab(WorkAreaManager contentManager)
        {
            var tab = TabControlVM.Tabs.FirstOrDefault((x) => x.WAManager == contentManager) ?? OpenTab(contentManager);
            TabControlVM.ActiveTab = tab;
        }
        public DiagramManager CreateDiagramManager()
        {
            var diagramManager = new DiagramManager(ActiveProject);
            diagramManager.Name = "диаграмма " + ActiveProject.Diagrams.Count;
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
                    OpenTab(CalculateGraphByCore());
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show("Ваша модель некорректна!\n" + e.Message);
                }
            }
        }
        private void OpenMathView()
        {

            if (TabControlVM.ActiveTab == null || TabControlVM.ActiveTab.WAManager is not DiagramManager)
            {
                System.Windows.MessageBox.Show("Откройте диаграмму, для которой необходимо построить мат. модель");
            }
            else
            {
                var diagram = (DiagramManager)TabControlVM.ActiveTab.WAManager;
                var math = diagram.MathVM ?? new MatViewManager(diagram);
                OpenTab(math);
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
                ActiveProject = Loader.InitProjectByPath(openFileDialog.FileName, _startWindowVM);
                _startWindowVM.AddProject(ActiveProject.FullName());
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
            CreateProject proj = new CreateProject(_startWindowVM);
            var window = proj as Window;
            var dialogResult = window.ShowDialog();

            if (dialogResult == true)
            {
                ActiveProject = Loader.InitProjectByPath(proj.ViewModel.FileName, _startWindowVM);
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

        public void Remove(IPropertyOwner obj)
        {
            if (obj is Project)
            {
                DeleteProject();
                
            }

            if (obj is DiagramManager diagramManager)
            {
                for (int i = 0; i < TabControlVM.Tabs.Count; i++)
                {
                    var tab = TabControlVM.Tabs[i];
                    if (tab.WAManager == diagramManager)
                    {
                        TabControlVM.Tabs.Remove(tab);
                        i--;
                    }
                }
                ActiveProject.Remove(diagramManager);
                return;
            }

            if (obj is ForesterNodeModel node)
            {
                ActiveProject.Remove(node);
            }
        }
        public void SetSelectedNode(ForesterNodeModel node)
        {
            foreach (var diagram in ActiveProject.Diagrams)
            {
                foreach (var nod in diagram.АllNodes)
                {
                    if (nod == node)
                    {
                        OpenOrCreateTab(diagram);
                        diagram.Content.ViewModel.ClearSelection();
                        node.IsSelected = true;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Создать проект
        /// создает и инициализирует активный проект
        /// </summary>
        private void SaveAsProject()
        {
            if (ActiveProject != null)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string path = dlg.FileName;
                    ActiveProject.Name = Path.GetFileNameWithoutExtension(path);
                    ActiveProject.PathToProject = Path.GetDirectoryName(path) + "\\" + ActiveProject.Name;
                    ActiveProject.SaveNewProject();
                    //  System.Windows.MessageBox.Show(path+": " + ActiveProject.Name+"  " + ActiveProject.PathToProject);
                }

            }


        }



    }
}