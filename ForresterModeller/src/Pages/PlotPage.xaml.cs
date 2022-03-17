using System;
using System.Drawing;
using System.Windows.Controls;
using ScottPlot;
using ScottPlot.Plottable;

namespace ForresterModeller.src.Pages
{
    /// <summary>
    /// Логика взаимодействия для PlotPage.xaml
    /// </summary>
    public partial class PlotPage : Page
    {
        public PlotPage()
        {
            InitializeComponent();
            FillPlot();
        }

        public void FillPlot()
        {
            InitializeComponent();
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            double[] dataX2 = { 1, 2, 3, 4, 5 };
            double[] dataY2 = { 1, 6, 11, 19, 10 };
            var a = new ScatterPlot(dataX, dataY);
            WpfPlot1.Plot.AddScatter(dataX, dataY, Color.Aqua, Single.Epsilon, Single.Epsilon, MarkerShape.asterisk, LineStyle.Solid, "DUR");
            WpfPlot1.Plot.AddScatter(dataX2, dataY2, Color.Blue, Single.Epsilon, Single.Epsilon, MarkerShape.asterisk, LineStyle.Solid, "DHR");
            WpfPlot1.Plot.YLabel("Объем товара (единицы)");
            WpfPlot1.Plot.XLabel("Время (недели)");
            WpfPlot1.Plot.Legend();
            WpfPlot1.Refresh();
        }
    }
}
