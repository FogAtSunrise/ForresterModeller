using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using ForesterNodeCore;
using ForresterModeller.src.ProjectManager.WorkArea;
using ReactiveUI;
using WpfMath.Controls;

namespace ForresterModeller.Windows.ViewModels
{
    public class MainWindowViewModel: ReactiveObject
    {
        public TabControlViewModel TabControlVM { get;  } = new();
        public PropertiesControlViewModel PropertiesVM { get; set; } = new();
        public ObservableCollection<FormulaControl> Formulas { get; set; }

        #region commands
        public ReactiveCommand<WorkAreaManager, Unit> OpenTab { get; }
        public ReactiveCommand<TabViewModel, Unit> CloseTab { get; }
        public ReactiveCommand<String, Unit> CalculateByCore { get; }
        public ReactiveCommand<Unit, Unit> OpenTestGraph { get; }

        #endregion

        public MainWindowViewModel()
        {
            OpenTab = ReactiveCommand.Create<WorkAreaManager>(o =>AddTab(new DiagramManager{Name = "Диаграмма12"}));
            CloseTab = ReactiveCommand.Create<TabViewModel>(o => TabControlVM.Tabs.Remove(o));
            CalculateByCore = ReactiveCommand.Create<String>(str => AddTab(CalculateGraphByCore()));
            OpenTestGraph = ReactiveCommand.Create<Unit>(u => AddTab(TestPlot()));
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
            PlotManager plotmodel = new(c, t, dt);
            plotmodel.XLabel = "Время (недели)FFF";
            plotmodel.YLabel = "Объем товара (единицы) ";
            plotmodel.Name = "График123";
            return plotmodel;
        }

        private PlotManager TestPlot()
        {
            PlotManager plotmodel = new();
            plotmodel.lines.Add(new PlotManager.Line(new double[]{1, 2, 3, 4}, new double[]{1, 2, 3, 4}, "Продуктивность студента"));
            plotmodel.XLabel = "Степень окончания семестра";
            plotmodel.YLabel = "Скорость сдачи лаб ";
            plotmodel.Name = "График продуктивности";
            return plotmodel;
        }


    }
}