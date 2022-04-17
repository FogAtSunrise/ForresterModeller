using System;
using ForresterModeller.src.ProjectManager.WorkArea;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using DynamicData;
using ForesterNodeCore;
using ForresterModeller.src.Interfaces;
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
       // private ActionTabViewModal OpenedPages;

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

        public NetworkViewModel GetNetworkViewModel()
        {
            var network = new NetworkViewModel();
            return network;
        }

        public void FillDiagram(NetworkView diag)
        {
            diag.ViewModel = GetNetworkViewModel();
        }
   
}
}
