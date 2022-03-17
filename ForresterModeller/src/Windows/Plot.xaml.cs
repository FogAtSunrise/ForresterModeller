using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ScottPlot;
using ScottPlot.Plottable;
using Color = System.Drawing.Color;

namespace ForresterModeller.src.Windows
{
    /// <summary>
    /// Логика взаимодействия для Plot.xaml
    /// </summary>
    public partial class Plot : Page
    {
        public Plot()
        {
            InitializeComponent();
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };  
            double[] dataX2 = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY2 = new double[] { 1, 6, 11, 19, 10 };
            var a = new ScatterPlot(dataX, dataY);
            WpfPlot1.Plot.AddScatter(dataX, dataY, Color.Aqua, Single.Epsilon, Single.Epsilon, MarkerShape.asterisk, LineStyle.Solid, "DUR");
            WpfPlot1.Plot.AddScatter(dataX2, dataY2, Color.Blue, Single.Epsilon, Single.Epsilon, MarkerShape.asterisk, LineStyle.Solid, "DHR");
            string[] labels = { "C#", "JAVA", "Python", "F#", "PHP" };
            WpfPlot1.Plot.YLabel("Объем товара (единицы)");
            WpfPlot1.Plot.XLabel("Время (недели)");
            WpfPlot1.Plot.Legend(true);
            WpfPlot1.Refresh();
        }
    }
}
