using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Pages.Properties;
using System.Text.Json.Nodes;

namespace ForresterModeller.src.Nodes.Models
{
    public class ConstantNodeViewModel : ForesterNodeModel
    {
        public override string TypeName
        {
            get => Resource.constType;
            set { }
        }

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

        public override JsonObject ToJSON()
        {
            JsonObject obj = new JsonObject()
            {
                ["Id"] = Id,
                ["Type"] = this.GetType().ToString(),
                ["Name"] = Name == null ? "" : "Name",
                ["FullName"] = FullName == null ? "" : FullName,
                ["Value"] = Value,
                //  ["OutputRate"] = OutputRate,
                ["Description"] = Description == null ? "" : Description

            };

            return obj;
        }

        public override bool FromJSON(JsonObject obj)
        {
            Id = obj!["Id"]!.GetValue<string>();
            Name = obj!["Name"]!.GetValue<string>();
            FullName = obj!["FullName"]!.GetValue<string>();
            Value = obj!["Value"]!.GetValue<float>();
            // OutputRate = obj!["OutputRate"]!.GetValue<string>();
            Description = obj!["Description"]!.GetValue<string>();
            return true;
        }
    }


}
