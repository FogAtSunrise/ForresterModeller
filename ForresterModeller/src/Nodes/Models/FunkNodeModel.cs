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

namespace ForresterModeller.src.Nodes.Models
{
    public class FunkNodeModel : ForesterNodeModel
    {
        public static string type = "FunkNodeModel";

        public override string TypeName => Resource.funcType;

        public string Funk { get; set; }
        public FunkNodeModel(string name, string fulname, string funk) : base()
        {
            this.Name = name;
            this.Funk = funk;
            this.FullName = fulname;
            var a = new ForesterNodeOutputViewModel();
            a.PortPosition = PortPosition.Right;
            this.Outputs.Add(a);

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
                ["Description"] = Description == null ? "" : Description
            };

            return obj;
        }

        public override void FromJSON(JsonObject obj)
        {
            Id = obj!["Id"]!.GetValue<string>();
            Name = obj!["Name"]!.GetValue<string>();
            FullName = obj!["FullName"]!.GetValue<string>();
            Funk = obj!["Funk"]!.GetValue<string>();
            Description = obj!["Description"]!.GetValue<string>();
    
        }

        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitFunc(this);
        }
    }
}
    
