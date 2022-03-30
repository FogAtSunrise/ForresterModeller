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

namespace ForresterModeller.src.Nodes.Models
{
    public class FunkNodeModel : ForesterNodeModel
    {
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
        public override ObservableCollection<Property> GetProperties()
        {
            var properties = base.GetProperties();
            properties.Add(new Property(Resource.equationType, Funk, (String str) => { Funk = str; }));
            //todo парсер на поля в уравнеии и их добавление в проперти
            return properties;
        }

        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitFunc(this);
        }
    }
}
