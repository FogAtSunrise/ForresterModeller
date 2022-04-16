using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Accessibility;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.Windows.ViewModels;
using ReactiveUI;
using ScottPlot;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    public class PlotManager : WorkAreaManager
    {
        private ContentControl _content;
        public override ContentControl Content
        {
            get => _content??GenerateActualPlot();
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }
        
        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var properties  = new ObservableCollection<PropertyViewModel>();
            properties.Add(new PropertyViewModel("Тип", TypeName));
            properties.Add(new PropertyViewModel("Название", Name, (string s) =>
            {
                Name = s;
                GenerateActualPlot();
            }));
            properties.Add(new PropertyViewModel("Ось абсцисс", XLabel, (String str) => {
                XLabel = str;
                GenerateActualPlot();
            }));
            properties.Add(new PropertyViewModel("Ось ординат", YLabel, (String str) => {
                YLabel = str;
                GenerateActualPlot();
            }));
            return properties;
        }

        public override string TypeName => "График"; 

        public PlotManager(){}
        /// <summary>
        /// Построить модель графа на основе данных от ядра
        /// </summary>
        /// <param name="resultMap"></param>
        /// <param name="t"></param>
        /// <param name="dt"></param>
        public PlotManager(Dictionary<string, double[]> resultMap, double t, double dt)
        {
            lines = new();
            int count = resultMap.First().Value.Length;
            double[] weeks = new double[count];
            if (count > 0)
                weeks[0] = 0;
            for (int i = 1; i < count; i++)
            {
                weeks[i] = weeks[i - 1] + dt;
            }
            foreach (var plot in resultMap)
            {
                double[] value = plot.Value;
                var line = new Line(weeks, value, plot.Key);
                lines.Add(line);
            }
        }
        /// <summary>
        /// Одна линия на графике
        /// </summary>
        public class Line
        {
            public Line(double[] x, double[] y, string description)
            {
                X = x;
                Y = y;
                Description = description;
            }
            public double[] X { get; }
            public double[] Y { get; }
            public bool IsVisible { get; set; } = true;
            public string Description { get; }
        }
        ///
        ///Информация, оторбараюжаяся на графике
        ///
        /// Перечень линий
        public ObservableCollection<Line> lines = new();
        /// <summary>
        /// Подписи для осей
        /// </summary>
        public String XLabel, YLabel;
        
        /// <summary>
        /// Получить график с актуальными изменениями
        /// </summary>
        /// <returns></returns>
        public ContentControl GenerateActualPlot()
        {
            WpfPlot WpfPlot1 = new();
            foreach (var line in lines)
            {
                WpfPlot1.Plot.AddScatter(line.X, line.Y, null,
                    Single.Epsilon, Single.Epsilon, MarkerShape.asterisk, LineStyle.Solid, line.Description);
            }
            WpfPlot1.Plot.YLabel(YLabel);
            WpfPlot1.Plot.XLabel(XLabel);
            WpfPlot1.Plot.Legend();
            WpfPlot1.Plot.Title(this.Name);
            WpfPlot1.Refresh();
            Content = WpfPlot1;
            return Content;
        }
        
    }
}