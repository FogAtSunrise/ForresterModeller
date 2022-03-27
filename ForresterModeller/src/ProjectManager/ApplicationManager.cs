﻿using System;
using ForresterModeller.src.ProjectManager.WorkArea;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
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
        //Project activeProject;
        private List<IForesterModel> allnodes = new();
        WorkAreaManager active;
        List<WorkAreaManager> opened = new();
        private IForesterModel activeModel;
        private List<ForesterNodeModel> diagramsNode = new();

        //todo сделать нормально из файла с данными о диаграмме
        /// <summary>
        /// Создает предствление конкретной диаграммы по списку ее узлов 
        /// </summary>
        /// <param name="nods"></param>
        /// <returns></returns>
        public NetworkViewModel GetNetworkViewModel(List<ForesterNodeModel> nods)
        {
            var network = new NetworkViewModel();
            foreach (var node in nods)
            {
                network.Nodes.Add(node);
            }
            return network;
        }

        public ApplicationManager()
        {
            var n1 = new ConstantNodeViewModel("DUR", "Задержка поставок констант", 12);
            n1.Description = "Стабильная константа";
            var n2 = new ConstantNodeViewModel();
            var n3 = new LevelNodeModel();
            n3.Description = "Невероятный уровень";
            //var n4 = new FunkNodeModel();
            allnodes.Add(n1);
            allnodes.Add(n2);
            allnodes.Add(n3);
            diagramsNode.Add(n1);
            diagramsNode.Add(n2);
            diagramsNode.Add(n3);
        }

        public void FillDiagram(NetworkView diag)
        {
            diag.ViewModel = GetNetworkViewModel(diagramsNode);
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

        /// <summary>
        /// Сформировать содержимое таба
        /// </summary>
        /// <returns></returns>
        public ContentControl CreateContentControl(string type)
        {
            ContentControl cc = null;

            if (type == "diagram")
            {
                NetworkView graf = new NetworkView() { Background = Brushes.AliceBlue };
                FillDiagram(graf);
                cc = graf;
            }
            else if (type == "plotter")
            {

                WpfPlot plot = new WpfPlot() { Name = "WpfPlot1" };
                FillPlot(plot);
                cc = plot;
            }

            return cc;
        }
    }
}