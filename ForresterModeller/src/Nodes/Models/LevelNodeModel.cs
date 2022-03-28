using System;
using System.Collections.ObjectModel;
using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Pages.Properties;
using ForresterModeller.src.Nodes.Viters;
using System.Text.Json.Nodes;

namespace ForresterModeller.src.Nodes.Models
{
    public class LevelNodeModel : ForesterNodeModel
    {
        public override string TypeName => Resource.levelType;

        public static string type = "LevelNodeModel";
      
        public string InputRate { get; set; }
        public string OutputRate { get; set; }

        public string StartValue { get; set; }

        public LevelNodeModel(string name, string fulname, string input, string output) : base()
        {
            this.Name = name;
            this.Id = name;
            this.InputRate = input;
            this.OutputRate = output;
            this.FullName = fulname;
         
            this.Description="";

            var a = new NodeOutputViewModel();
            a.PortPosition = PortPosition.Right;
            this.Outputs.Add(a);

            a = new NodeOutputViewModel();
            a.PortPosition = PortPosition.Right;
            Outputs.Add(a);

            var b = new NodeInputViewModel();
            b.PortPosition = PortPosition.Left;
            Inputs.Add(b);
        }
        public LevelNodeModel() : this("LVL", "Уровень", "1", "1") { }
        static LevelNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("level"), typeof(IViewFor<LevelNodeModel>));
        }

        public override JsonObject ToJSON() {
            JsonObject obj = new JsonObject() {
                ["Id"] = Id,
                ["Type"] = type,
                ["Name"] = Name,
                
                ["FullName"] = FullName,
                ["InputRate"] = InputRate,
                ["OutputRate"] = OutputRate,
                ["Description"] = Description == null ? "" : Description
            };

            return obj;
        }

        public override bool FromJSON(JsonObject obj) {
            Id = obj!["Id"]!.GetValue<string>();
            Name = obj!["Name"]!.GetValue<string>();
            
            FullName = obj!["FullName"]!.GetValue<string>();
            InputRate = obj!["InputRate"]!.GetValue<string>();
            OutputRate = obj!["OutputRate"]!.GetValue<string>();
            Description = obj!["Description"]!.GetValue<string>();
            return true; }



        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitLevel(this);
        }
    }

}
