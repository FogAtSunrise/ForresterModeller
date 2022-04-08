using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using DynamicData;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.Windows.ViewModels;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    public class DiagramManager : WorkAreaManager
    {

        private IPropertyOwner _activeItem;
        public override IPropertyOwner ActiveOwnerItem
        {
            get => _activeItem ?? this;
            set
            {
                this.RaiseAndSetIfChanged(ref _activeItem, value);
                OnPropertySelected(_activeItem);
            }
        }
        private NetworkView _contentView;
        private ObservableCollection<ForesterNodeModel> _selectedNodes = new();
        public ObservableCollection<ForesterNodeModel> SelectedNodes
        {
            get => _selectedNodes;
            set => this.RaiseAndSetIfChanged(ref _selectedNodes, value);
        }
        public override ContentControl Content => _contentView ?? CreateNetworkView();
        public override string TypeName => "Диаграмма потоков";
        public double DeltaTime { get; set; }
        public double AllTime { get; set; }

        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var prop = base.GetProperties();
            prop.Add(new PropertyViewModel("Период исследования", AllTime.ToString(), s => AllTime = Double.Parse(s)));

            prop.Add(new PropertyViewModel("DT", DeltaTime.ToString(), Aaa));
            return prop;
        }

        private void Aaa(string a)
        {
            DeltaTime = Double.Parse(a);
        }
        private void AddDragNode(ForesterNodeModel node)
        {
            node.PropertyChanged += NodeOnPropertyChanged;
        }

        public NetworkView CreateNetworkView()
        {
            _contentView = new NetworkView() { Background = Brushes.AliceBlue };
            var network = new NetworkViewModel();
            network.NodeDeletedEvent += (list) =>
            {
                foreach (var node in list)
                {
                    SelectedNodes.Remove((ForesterNodeModel)node);
                }
                OnPropertySelected(this);
            };
            ///
            this._contentView.Drop += (o, e) => {
                AddDragNode((ForesterNodeModel)e.Data.GetData("nodeVM")); 
            };
            _contentView.ViewModel = network;
            return _contentView;
        }

        private void NodeOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ForesterNodeModel.IsSelected))
            {
                var node = (ForesterNodeModel)sender;
                if (node.IsSelected)
                    SelectedNodes.Add(node);
                else if (SelectedNodes.Contains(node))
                {
                    SelectedNodes.Remove(node);
                }
                if (SelectedNodes.Count == 1)
                {
                    ActiveOwnerItem = node;
                }
                else ActiveOwnerItem = this;
                OnPropertySelected(ActiveOwnerItem);
            }
        }

    }
}