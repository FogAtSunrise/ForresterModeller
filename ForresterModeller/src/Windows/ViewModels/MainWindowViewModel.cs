using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reactive;
using ForesterNodeCore;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.ProjectManager.WorkArea;
using ReactiveUI;
using WpfMath.Controls;

namespace ForresterModeller.ViewModels
{
    public class MainWindowViewModel: ReactiveObject
    {
        public TabControlViewModel TabControlVM { get; set; } = new();
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
            OpenTab = ReactiveCommand.Create<WorkAreaManager>(o => TabControlVM.AddTab(new DiagramManager(){Name = "Новый таб"}));
            CloseTab = ReactiveCommand.Create<TabViewModel>(o => TabControlVM.Tabs.Remove(o));
            CalculateByCore = ReactiveCommand.Create<String>(str => TabControlVM.AddTab(CalculateGraphByCore()));
            OpenTestGraph = ReactiveCommand.Create<Unit>(u => TabControlVM.AddTab(TestPlot()));
            TabControlVM.PropertyChanged += TabControlUpdate;
        }

        private void TabControlUpdate(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TabControlVM.ActiveTab))
            {
                PropertiesVM.ActiveItem = TabControlVM.ActiveTab.WAManager;
            }
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