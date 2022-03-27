using System;
using ForresterModeller.src.ProjectManager.WorkArea;
using System.Collections.Generic;
using DynamicData;
using ForresterModeller.src.Nodes.Models;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ScottPlot;
using ScottPlot.Plottable;


namespace ForresterModeller.src.ProjectManager
{
    class ApplicationManager
    {
        Project activeProject;

        WorkAreaManager active;
        List<WorkAreaManager> opened;
        private IForesterModel activeModel;

        private List<NetworkViewModel> diagramsList;

        public ApplicationManager()
        {
           
        }
        public void FillDiagram(NetworkView diag)
        {

            var network = new NetworkViewModel();
            var node1 = new ConstantNodeViewModel();
            network.Nodes.Add(node1);
            var node2 = new LevelNodeModel();
            network.Nodes.Add(node2);
            diag.ViewModel = network;
        }

        public void FillPlot(WpfPlot WpfPlot1)
        {
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            double[] dataX2 = { 1, 2, 3, 4, 5 };
            double[] dataY2 = { 1, 6, 11, 19, 10 };
            var a = new ScatterPlot(dataX, dataY);
            WpfPlot1.Plot.AddScatter(dataX, dataY, System.Drawing.Color.Aqua, Single.Epsilon, Single.Epsilon, MarkerShape.asterisk, LineStyle.Solid, "DUR");
            WpfPlot1.Plot.AddScatter(dataX2, dataY2, System.Drawing.Color.Blue, Single.Epsilon, Single.Epsilon, MarkerShape.asterisk, LineStyle.Solid, "DHR");
            WpfPlot1.Plot.YLabel("Объем товара (единицы)");
            WpfPlot1.Plot.XLabel("Время (недели)");
            WpfPlot1.Plot.Legend();
            WpfPlot1.Refresh();
        }

        public void FillTabItem(string fileName)
        {

        }
        public void ObjectSelected(IForesterModel model)
        {

        }
        // Frame tools;//? 
    }
}
