using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using System.Windows.Media;
using DynamicData;
using ForresterModeller.src.Interfaces;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.ProjectManager.miniParser;
using ForresterModeller.src.Windows.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    public class DiagramManager : WorkAreaManager
    {

        private Project _project;
        public string FullName => PathToFile + "\\" + Name;

        public DiagramManager(Project project) { _project = project; }
        public DiagramManager(string name, Project project) : this(project)
        {
            Name = name;
        }

        public void JsonToDiagram(JsonObject json)
        {
            try
            {
                Name = json!["Name"]!.GetValue<string>();
                AllTime = json!["Time"]!.GetValue<float>();
                DeltaTime = json!["Delta"]!.GetValue<float>();
            }
            catch
            {
                MessageBox.Show("Не верно выбран файл проекта");
            }

            //ЗДЕСЬ ЛОГИКА, КОТОРАЯ ПО JSON ЗАПОЛНЯЕТ ПОЛЯ ДИАГРАММЫ

            var nodes = json!["Nodes"];
            foreach (var node in nodes.AsArray())
            {
                ForesterNodeModel newNode = null;


                switch (node!["Type"].AsValue().ToString())
                {
                    case "ChouseNodeModel":
                        newNode = new ChouseNodeModel();
                        break;
                    case "ConstantNodeViewModel":
                        newNode = new ConstantNodeViewModel();
                        break;
                    case "CrossNodeModel":
                        newNode = new CrossNodeModel();
                        break;
                    case "DelayNodeModel":
                        newNode = new DelayNodeModel();
                        break;
                    case "FunkNodeModel":
                        newNode = new FunkNodeModel();
                        break;
                    case "LevelNodeModel":
                        newNode = new LevelNodeModel();
                        break;
                    case "LinkNodeModel":
                        newNode = new LinkNodeModel(_project);
                        break;

                }

                newNode.FromJSON(node.AsObject());
                newNode.PropertyChanged += NodeOnPropertyChanged;
                this.Content.ViewModel.Nodes.Add(newNode);
            }

            ((ForesterNetworkViewModel)this.Content.ViewModel).AutoConect();
            UpdateNodes();


        }

        public JsonObject DiagramToJson()
        {
            UpdateNodes();
            JsonArray nodesJson = new();

            foreach (var node in АllNodes)
            {
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

        public void UpdateNodes()
        {
            АllNodes = Content.ViewModel.Nodes.Items.Select(x => (ForesterNodeModel)x);
        }
        private IEnumerable<ForesterNodeModel> _allnodes;
        public IEnumerable<ForesterNodeModel> АllNodes
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
            prop.Add(new PropertyViewModel("Период исследования", AllTime.ToString(), s => AllTime = Utils.GetDouble(s), Pars.CheckConst));

            prop.Add(new PropertyViewModel("DT", DeltaTime.ToString(), s => DeltaTime = Utils.GetDouble(s), Pars.CheckConst));
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
            var network = new ForesterNetworkViewModel();
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
            this._contentView.Drop += (o, e) =>
            {
                AddDragNode((ForesterNodeModel)e.Data.GetData("nodeVM"));
                UpdateNodes();
            };

            this._contentView.Drop += (o, e) =>
            {
                ForesterNodeModel node = (ForesterNodeModel)e.Data.GetData("nodeVM");

                if (node is LinkNodeModel)
                {
                    if (((LinkNodeModel)node).Modegel == ((NetworkView)o).ViewModel)
                    {
                        e.Handled = true;
                    }
                }
            };
            _contentView.ViewModel = network;
            return _contentView;
        }

        private void NodeOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ForesterNodeModel.IsSelected))
            {
                var node = (ForesterNodeModel)sender;
                if (node != null && node.IsSelected)
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