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
using ForresterModeller.Windows.ViewModels;

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
            this.Id = name;
            this.Funk = funk;
            this.FullName = fulname;
            var a = new NodeOutputViewModel();
            a.PortPosition = PortPosition.Right;
            this.Outputs.Add(a);

            var b = new NodeInputViewModel();
            b.Name = "x";
            b.PortPosition = PortPosition.Left;
            Inputs.Add(b);
        }
        public FunkNodeModel() : this("FUN", "функция", "x") { }
        static FunkNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("funk"), typeof(IViewFor<FunkNodeModel>));
        }
        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var properties = base.GetProperties();
            properties.Add(new PropertyViewModel(Resource.equationType, Funk, (String str) => { Funk = str; }));
            //todo парсер на поля в уравнеии и их добавление в проперти
            return properties;
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
    
