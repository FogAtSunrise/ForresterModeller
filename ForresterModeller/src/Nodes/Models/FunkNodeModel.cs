using System;
using System.Collections.ObjectModel;
using DynamicData;
using ForresterModeller;
using ForresterModeller.src.Nodes.Models;
using NodeNetwork.ViewModels;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Nodes.Viters;
using System.Text.Json.Nodes;
using System.Linq;
using ForresterModeller.src.Windows.ViewModels;
using System.Windows;
using System.Reactive.Linq;
using System.Reactive;

namespace ForresterModeller.src.Nodes.Models
{
    public class FunkNodeModel : ForesterNodeModel
    {
        public static string type = "FunkNodeModel";
        private ForesterNodeOutputViewModel _out;
        public override string TypeName => Resource.funcType;
        public string Funk { get; set; }
        public FunkNodeModel(string name, string fulname, string funk) : base()
        {
            this.Name = name;
            this.Funk = funk;
            this.FullName = fulname;
            _out = new ForesterNodeOutputViewModel();
            _out.PortPosition = PortPosition.Right;
            _out.Name = name;
            this.Outputs.Add(_out);

            RefreshInput();
        }
        public FunkNodeModel() : this("FUN", "функция", "(x+y)/2") { }
        static FunkNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("funk"), typeof(IViewFor<FunkNodeModel>));
        }
        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var properties = base.GetProperties();
            properties.RemoveAt(0);

            properties.Insert(0,new PropertyViewModel(Resource.name, Name, (String str) => {
                Name = str;
                Outputs.Items.First().Name = str;
            }));


            properties.Add(new PropertyViewModel(Resource.equationType, Funk, (String str) => { Funk = str; RefreshInput(); }));
            //todo парсер на поля в уравнеии и их добавление в проперти
            return properties;
        }

        // public override MathViewModel GetMathView() { return new MathViewModel(Name, Funk); }
        public void RefreshInput()
        {
            var vars = ForesterNodeCore.Program.GetArgs(this.Funk);
            foreach(var _var in vars)
            {
                if(this.Inputs.Items.ToList().FindAll(port => port.Name == _var).Count == 0)
                {
                    var b = new NodeInputViewModel();
                    b.Name = _var;
                    b.PortPosition = PortPosition.Left;
                    Inputs.Add(b);
                }
            }

            foreach (var _var in this.Inputs.Items.ToList())
            {
                if (! vars.ToList().Contains(_var.Name) )
                {
                    Inputs.Remove(_var);
                }
            }


        }
        public override JsonObject ToJSON()
        {
            JsonObject obj = new JsonObject()
            {
                ["Id"] = Id ,
                ["Type"] = type,
                ["Name"] = Name == null ? "" :Name,            
                ["FullName"] = FullName == null ? "" : FullName,
                ["Funk"] = Funk == null ? "" : Funk,
                ["Description"] = Description == null ? "" : Description,
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
            _out.Name = obj!["Name"]!.GetValue<string>();
            FullName = obj!["FullName"]!.GetValue<string>();
            Funk = obj!["Funk"]!.GetValue<string>();
            Description = obj!["Description"]!.GetValue<string>();
            RefreshInput();
            Position = new Point(obj!["PositionX"]!.GetValue<double>(), obj!["PositionY"]!.GetValue<double>());
            var conList = obj!["Conects"].AsArray();

            foreach (var con in conList)
            {
                if (con is null)
                {
                    DumpConections.Add(null);
                }
                else
                {
                    DumpConections.Add(new ConectionModel(con!["SourceId"].GetValue<string>(), con!["PointName"].GetValue<string>())); ;
                }
            }
        }
        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitFunc(this);
        }
    }

    public class MaxNodeModel : ChouseNodeModel
    {
        public static string type = "ChouseNodeModel";
        public MaxNodeModel() : base("max", "max", "(first + second + abs(first-second))/2")
        {
        }
        static MaxNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("chouse"), typeof(IViewFor<MaxNodeModel>));
        }
    }

    public class MinNodeModel : ChouseNodeModel
    {
        public static string type = "ChouseNodeModel";
        public MinNodeModel() : base("min", "min", "(first + second - abs(first-second))/2")
        {
        }
        static MinNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("chouse"), typeof(IViewFor<MinNodeModel>));
        }
    }


}
    
