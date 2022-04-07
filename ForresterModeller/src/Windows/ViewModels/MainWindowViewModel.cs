using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows;
using System.Windows.Forms;
using ForesterNodeCore;
using ForresterModeller.src.ProjectManager;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Nodes.Viters;
using ForresterModeller.src.ProjectManager.WorkArea;
using NodeNetwork.Views;
using ReactiveUI;
using WpfMath.Controls;
using ForresterModeller.src.Windows;
using ForresterModeller.src.Windows.ViewModels;
using Splat;

namespace ForresterModeller.Windows.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public TabControlViewModel TabControlVM { get; } = new();
        public PropertiesControlViewModel PropertiesVM { get; set; } = new();
        public ObservableCollection<FormulaControl> Formulas { get; set; }
 
        public Project activeProject { get; set; }


        #region commands
        //Создать новую диаграмму
        public ReactiveCommand<WorkAreaManager, Unit> CreateDiagram { get; }
        /// <summary>
        /// Открыть элемент по имени
        /// </summary>
        public ReactiveCommand<String, Unit> OpenTab { get; }
        public ReactiveCommand<TabViewModel, Unit> CloseTab { get; }
        public ReactiveCommand<String, Unit> CalculateByCore { get; }
        public ReactiveCommand<Unit, Unit> OpenTestGraph { get; }

        /// <summary>
        /// Открыть существующий проект
        /// Открывает диаологовое окно, по выбранному json инициализирует активный проект необходимыми данными
        /// </summary>
        public ReactiveCommand<Unit, Unit> InitProjectByPath { get; }

        /// <summary>
        /// Создать проект
        /// создает и инициализирует активный проект
        /// </summary>
        public ReactiveCommand<Unit, Unit> CreateNewProject { get; }

        #endregion
        public MainWindowViewModel()
        {
            CreateDiagram = ReactiveCommand.Create<WorkAreaManager>(o => AddTab(new DiagramManager { Name = "Диаграмма12" }));
            OpenTab = ReactiveCommand.Create<String>(s => AddTab(new DiagramManager { Name = "Диаграмма12" }));
            CloseTab = ReactiveCommand.Create<TabViewModel>(o => TabControlVM.Tabs.Remove(o));
            CalculateByCore = ReactiveCommand.Create<String>(str =>
            {
                try
                {
                    AddTab(CalculateGraphByCore());
                }
                catch
                {
                    System.Windows.MessageBox.Show("Ваша модель некорректна!");
                }
            });
            OpenTestGraph = ReactiveCommand.Create<Unit>(u => AddTab(TestPlot()));

            InitProjectByPath = ReactiveCommand.Create<Unit>(u => {

                if (activeProject != null)
                {
                    activeProject.SaveOldProject();
                    System.Windows.MessageBox.Show("Сохранился текущий активный проект " + activeProject.getName());////////////////////////////////
                }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы json|*.json";
            openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog()==DialogResult.OK)
                {
                    activeProject = Loader.InitProjectByPath(openFileDialog.FileName);
                    if (activeProject != null)
                        System.Windows.MessageBox.Show("Открылся " + activeProject.getName());////////////////////////////////

                    //ИЗМЕНИТЬ СОДЕРЖИМОЕ ОКНА ЕЩЕ
                }
            });


            CreateNewProject = ReactiveCommand.Create<Unit>(u => {

                if (activeProject != null)
                {
                    activeProject.SaveOldProject();
                    System.Windows.MessageBox.Show("Сохранился текущий активный проект " + activeProject.getName());////////////////////////////////
                }
               
                CreateProject proj = new CreateProject();
                var window = proj as Window;
                var dialogResult = window.ShowDialog();

                if (dialogResult == true)

                {

                    //System.Windows.MessageBox.Show("Открылсяzzzzzzzzzzzzz " + proj.ViewModel.FileName);
                    activeProject = Loader.InitProjectByPath(proj.ViewModel.FileName);
                    if (activeProject != null)
                        System.Windows.MessageBox.Show("Открылся " + activeProject.getName());////////////////////////////////
                    
                    //ИЗМЕНИТЬ СОДЕРЖИМОЕ ОКНА ЕЩЕ
                }
            });

        }

        /// <summary>
        /// ДДОБАВЛЯТЬ ВКЛАДКИ ТОЛЬКО ЧЕРЕЗ ЭТОТ МЕТОД!!!
        /// иначе окно пропертей не будет обновляться
        /// </summary>
        /// <param name="contentManager"></param>
        private void AddTab(WorkAreaManager contentManager)
        {
            contentManager.PropertySelectedEvent += sender => PropertiesVM.ActiveItem = sender;
            TabControlVM.AddTabFromWAManager(contentManager);

        }

        private PlotManager CalculateGraphByCore()
        {
            if (TabControlVM.ActiveTab.WAManager is not DiagramManager)
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
            PlotManager plotmodel = new(c, t, dt);
            plotmodel.XLabel = "Время (недели)FFF";
            plotmodel.YLabel = "Объем товара (единицы) ";
            plotmodel.Name = "График123";
            return plotmodel;
        }

        private PlotManager TestPlot()
        {
            PlotManager plotmodel = new();
            plotmodel.lines.Add(new PlotManager.Line(new double[] { 1, 2, 3, 4 }, new double[] { 1, 2, 3, 4 }, "Продуктивность студента"));
            plotmodel.XLabel = "Степень окончания семестра";
            plotmodel.YLabel = "Скорость сдачи лаб ";
            plotmodel.Name = "График продуктивности";
            return plotmodel;
        }


    }
}