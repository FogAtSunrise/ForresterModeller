using System;
using System.Collections.ObjectModel;
using DynamicData;
using ForresterModeller;
using ForresterModeller.src.Nodes.Models;
using NodeNetwork.ViewModels;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Pages.Properties;
using ForresterModeller.src.Nodes.Viters;
using System.Text.Json.Nodes;

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
            b.Name = "boopa";
            b.PortPosition = PortPosition.Left;
            Inputs.Add(b);
        }
        public FunkNodeModel() : this("LVL", "Уровень", "1") { }
        static FunkNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("funk"), typeof(IViewFor<FunkNodeModel>));
        }
        public override ObservableCollection<Property> GetProperties()
        {
            var properties = base.GetProperties();
            properties.Add(new Property(Resource.equationType, Funk, (String str) => { Funk = str; }));
            //todo парсер на поля в уравнеии и их добавление в проперти
            return properties;
        }

        public override JsonObject ToJSON()
        {
            JsonObject obj = new JsonObject()
            {
                ["Id"] = Id ,
                ["Type"] = type,
                ["Name"] = Name == null ? "" :"Name",            
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
    }
}
    

    public class ChouseNodeModel:FunkNodeModel
    {
    public static string type = "ChouseNodeModel";
    public ChouseNodeModel(string name, string fulname, string funk):base(name, fulname, funk)
        {

        }

        public ChouseNodeModel() : base()
        {

        }

        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitFunc(this);
        }

        static ChouseNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("chouse"), typeof(IViewFor<ChouseNodeModel>));
        }
    }

