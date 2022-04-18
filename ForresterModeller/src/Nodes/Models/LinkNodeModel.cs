using System;
using System.Collections.ObjectModel;
using DynamicData;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Nodes.Viters;
using System.Text.Json.Nodes;
using System.Linq;
using ForresterModeller.src.Windows.ViewModels;
using System.Windows;
using System.Reactive.Linq;
using System.Reactive;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using NodeNetwork;
using ForresterModeller.src.ProjectManager.WorkArea;
using ForresterModeller.src.ProjectManager;

namespace ForresterModeller.src.Nodes.Models
{
    public class LinkNodeModel : ForesterNodeModel
    {
        public static string type = "LinkNodeModel";
        private Project _diagrams;
        public string Salt => Id;
        public ForesterNetworkViewModel Modegel => (ForesterNetworkViewModel)_diagrams.Diagrams.FirstOrDefault(a => a.Name == this.Name).Content.ViewModel;
                
        public override string TypeName => Resource.linkDiagramm;
        public LinkNodeModel(Project diagram, string name) : base()
        {
            _diagrams = diagram;
            this.Name = name;

           _diagrams.Diagrams.FirstOrDefault(a => a.Name == this.Name).PropertyChanged += (sender, e) =>
           {
               if (e.PropertyName == "Name")
               {
                   this.Name = ((DiagramManager)sender).Name;
               }
           };


            Modegel.PropertyChanged += (sender, e) => {
                if (e.PropertyName == "LatestValidation") RefreshInput(); };


            RefreshInput();
        }
        static LinkNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("level"), typeof(IViewFor<LinkNodeModel>));
        }
        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var command = ReactiveCommand.CreateFromObservable<Unit, int>(_ => Observable.Return(42).Delay(TimeSpan.FromSeconds(2)));
            Modegel.PropertyChanged += (sender, e) => RefreshInput();
            var properties = new ObservableCollection<PropertyViewModel>();
            properties.Add(new PropertyViewModel(Resource.name, Name));
            properties.Add(new PropertyViewModel(Resource.type, TypeName));
            return properties;
        }
        public void RefreshInput()
        {
            Inputs.Clear();
            Outputs.Clear();

            foreach (var node in Modegel.Nodes.Items )
            {
                foreach (var inp in node.Inputs.Items)
                {
                    if (inp.Connections.Count == 0 && !Inputs.Items.Contains(inp))
                    {
                        this.Inputs.Add(new LincNodeModelInputRate() { Name = inp.Name, PortPosition = PortPosition.Left, Target = (ForesterNodeModel)inp.Parent});
                    }
                }

                foreach (var inp in node.Outputs.Items )
                {
                    if (inp.Connections.Count == 0 && !Outputs.Items.Contains(inp))
                    {
                        this.Outputs.Add(new ForesterNodeOutputViewModel() {OutFunc = () => ((ForesterNodeOutputViewModel)inp).OutputValue + Salt, Name = inp.Name} );
                    }
                }
            }
        }
        public override JsonObject ToJSON()
        {
            JsonObject obj = new JsonObject()
            {
                ["Id"] = Id,
                ["Type"] = type,
                ["Name"] = Name == null ? "" : Name,
                ["PositionX"] = Position.X,
                ["PositionY"] = Position.Y
            };

            JsonArray con = new();


            foreach (var inputs in this.Inputs.Items)
            {
                if (inputs.Connections.Items.Any())
                {
                    con.Add(new ConectionModel(inputs));
                }
                else
                {
                    con.Add(null);
                }
            }
            obj.Add("Conects", con);
            return obj;
        }
        public override void FromJSON(JsonObject obj)
        {
            Id = obj!["Id"]!.GetValue<string>();
            Name = obj!["Name"]!.GetValue<string>();
            FullName = obj!["FullName"]!.GetValue<string>();
            Description = obj!["Description"]!.GetValue<string>();
            RefreshInput();
            Position = new Point(obj!["PositionX"]!.GetValue<double>(), obj!["PositionY"]!.GetValue<double>());
            var conList = obj!["Conects"].AsArray();

            foreach (var con in conList)
            {
                if (con is null)
                {
                    _dump_conections.Add(null);
                }
                else
                {
                    _dump_conections.Add(new ConectionModel(con!["SourceId"].GetValue<string>(), con!["PointName"].GetValue<string>())); ;
                }
            }
        }
        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitLink(this);
        }
    }



    public class LincNodeModelInputRate : NodeInputViewModel
    {

        public ForesterNodeModel Target { get; set; }
        static LincNodeModelInputRate()
        {
            Splat.Locator.CurrentMutable.Register(() => new NodeInputView(), typeof(IViewFor<LincNodeModelInputRate>));
        }

    }

}
    
