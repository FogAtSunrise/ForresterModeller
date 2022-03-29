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
    public class DiagramManager : WorkAreaManager
    {
        public override IPropertyChangable ActiveModel { get; }
        private NetworkView _contentView;

        public override ContentControl Content => _contentView ?? GetNetworkView();

        public override string TypeName => "Диаграмма потоков";

        public NetworkView GetNetworkView()
        {
            _contentView = new NetworkView() { Background = Brushes.AliceBlue };
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
            //network.PropertyChanged += Network_PropertyChanged;
            _contentView.ViewModel = network;
            return _contentView;
        }

        private void Network_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}