using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using WPFtest1.src.Nodes.Views;

namespace WPFtest1.src.Nodes.Models
{
    public class ConstantNodeViewModel : ForesterNodeModel
    {
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
            this.FullName = fulname;
            this.Code = name;
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
    }


}
