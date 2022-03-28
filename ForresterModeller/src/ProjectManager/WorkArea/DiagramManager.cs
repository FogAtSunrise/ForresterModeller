using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using DynamicData;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Pages.Properties;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    public class DiagramManager : IWorkAreaManager
    {
        public string Name { get; set; }
        public string PathToFile { get; set; }
        public IPropertyChangable ActiveModel { get; }
        public ContentControl Content => GetNetworkView();

        public ObservableCollection<Property> GetProperties()
        {
            var properties = new ObservableCollection<Property>();

            properties.Add(new Property("Тип", "Диаграмма"));
            properties.Add(new Property("Название", Name, (String str) => { Name = str; }));
            return properties;
        }

        public NetworkView GetNetworkView()
        {
            NetworkView graf = new NetworkView() { Background = Brushes.AliceBlue };
            var network = new NetworkViewModel();
            ///
            ///
            List<ForesterNodeModel> nods = new();
            var n1 = new ConstantNodeViewModel("DUR", "Задержка поставок констант", 12);
            n1.Description = "Стабильная константа";
            var n2 = new ConstantNodeViewModel();
            var n3 = new LevelNodeModel();
            n3.Description = "Невероятный уровень";
            //var n4 = new FunkNodeModel();
            nods.Add(n1);
            nods.Add(n2);
            nods.Add(n3);
          
            ///
            foreach (var node in nods)
            {
                network.Nodes.Add(node);
            }
            graf.ViewModel = network;
            return graf;
        }
    }
}