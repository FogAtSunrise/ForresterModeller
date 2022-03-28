using System;
using ForresterModeller.src.ProjectManager.WorkArea;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using DynamicData;
using ForesterNodeCore;
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
      //  ActionTabItem active;
        private ActionTabViewModal OpenedPages;

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

        public void OpenNewTab()
        {

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


        public WpfPlot ExecuteCore()
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
            var plotmodel = new PlotManager(c, t, dt);
            plotmodel.XLabel = "Время (недели)FFF";
            plotmodel.YLabel = "Объем товара (единицы) ";
            return plotmodel.GenerateActualPlot();
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
                cc = ExecuteCore();
            }

            return cc;
        }


    
}
}
