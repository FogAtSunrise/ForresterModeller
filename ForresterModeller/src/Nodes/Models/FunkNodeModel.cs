using System;
using System.Collections.ObjectModel;
using DynamicData;
using ForresterModeller;
using ForresterModeller.src.Nodes.Models;
using NodeNetwork.ViewModels;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Pages.Properties;

namespace ForresterModeller.src.Nodes.Models
{
    public class FunkNodeModel : ForesterNodeModel
    {
        public override string TypeName { 
            get => Resource.funcType;
            set {}
        }
        public string Funk { get; set; }
        public FunkNodeModel(string name, string fulname, string funk) : base()
        {
            this.Name = name;
            this.Id = name;
            this.Funk = funk;
            this.FullName = fulname;
            var a = new NodeOutputViewModel();
            a.PortPosition = PortPosition.Left;
            this.Outputs.Add(a);

            for (int i = 0; i < 5; i++)
            {
                var b = new NodeInputViewModel();
                b.PortPosition = PortPosition.Circle;
                Inputs.Add(b);
            }
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
    }


}
