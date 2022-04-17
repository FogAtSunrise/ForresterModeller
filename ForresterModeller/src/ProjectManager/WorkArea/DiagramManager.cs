using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using DynamicData;
using ForresterModeller.src.Interfaces;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Windows.ViewModels;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    public class DiagramManager : WorkAreaManager
    {
        public DiagramManager() { }
 

        public DiagramManager(string name)
        {
            Name = name;
        }

        public void JsonToDiagram(JsonObject json)
        {
            try
            {
                Name = json!["Name"]!.GetValue<string>();

            }
            catch
            {
                MessageBox.Show("Не верно выбран файл проекта");
            }

            //ЗДЕСЬ ЛОГИКА, КОТОРАЯ ПО JSON ЗАПОЛНЯЕТ ПОЛЯ ДИАГРАММЫ

        }

        public JsonObject DiagramToJson()
        {
            JsonArray nodesJson = new();
            
            foreach (var node in GetAllNodes) {
                nodesJson.Add(node.ToJSON());
            }

            JsonObject json = new JsonObject
            {
                ["Name"] = Name,
                ["Time"] = AllTime,
                ["Delta"] = DeltaTime
            };

            json.Add("Nodes", nodesJson);

            return json;
        }

        private void UpdateNodes()
        {
            GetAllNodes = Content.ViewModel.Nodes.Items.Select(x => (ForesterNodeModel)x);
        }
        private IEnumerable<ForesterNodeModel> _allnodes;
        public IEnumerable<ForesterNodeModel> GetAllNodes
        {
            get => _allnodes;
            set { this.RaiseAndSetIfChanged(ref _allnodes, value); }
        }

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
        public override NetworkView Content => _contentView ?? CreateNetworkView();
        public override string TypeName => "Диаграмма потоков";
        public double DeltaTime { get; set; } = 0.1;
        public double AllTime { get; set; } = 10;

        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var prop = base.GetProperties();
            prop.Add(new PropertyViewModel("Период исследования", AllTime.ToString(), s => AllTime = Utils.GetDouble(s)));

            prop.Add(new PropertyViewModel("DT", DeltaTime.ToString(), s =>DeltaTime = Utils.GetDouble(s)));
            return prop;
        }

        private void AddDragNode(ForesterNodeModel node)
        {
            node.PropertyChanged += NodeOnPropertyChanged;
        }

        public NetworkView CreateNetworkView()
        {
            _contentView = new NetworkView() { Background = Brushes.AliceBlue };
            _contentView.NodeAddedEvent += (sender, args) => UpdateNodes();
            var network = new NetworkViewModel();
            network.NodeDeletedEvent += (list) =>
            {
                foreach (var node in list)
                {
                    SelectedNodes.Remove((ForesterNodeModel)node);
                }
                OnPropertySelected(this);
                UpdateNodes();
            };
            ///
            this._contentView.Drop += (o, e) => {
                AddDragNode((ForesterNodeModel)e.Data.GetData("nodeVM"));
                UpdateNodes();
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