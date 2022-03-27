using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Pages.Properties;
using ForresterModeller.src.Nodes.Viters;

namespace ForresterModeller.src.Nodes.Models
{
    public class ConstantNodeViewModel : ForesterNodeModel
    {
        public override string TypeName => Resource.constType;

        /// <summary>
        /// Значение всех констант по умолчанию
        /// </summary>
        public static float DefaultValue { get; set; } = 1;

        /// <summary>
        /// Значение текщей констнты
        /// </summary>
        public float Value { get; set; }

        
        public ConstantNodeViewModel(string name, string fulname, float value) : base()
        {
            this.Name = name;
            this.Id = name;
            this.Value = value;
            var a = new NodeOutputViewModel();
            a.PortPosition = PortPosition.Centr;
            this.Outputs.Add(a);
        }
        public ConstantNodeViewModel():this("DT", "DT", DefaultValue) { }
        static ConstantNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("constant"), typeof(IViewFor<ConstantNodeViewModel>));
        }

        public override ObservableCollection<Property> GetProperties()
        {
            var properties = base.GetProperties();
            //todo validation
            properties.Add(new Property( Resource.value,  Value.ToString(), (String str) => { Value = float.Parse(str); }));
            return properties;
        }

        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitConstant(this);
        }
    }


}
