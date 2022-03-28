using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Nodes.Viters;
using System.Text.Json.Nodes;

namespace ForresterModeller.src.Nodes.Models
{
    public class DelayNodeModel : ForesterNodeModel
    {
        public static string type = "DelayNodeModel";
        public override string TypeName => Resource.levelType;

        public string InputRate { get; set; }
        
        public string OutputRateName { get; set; }

        public int DeepDelay { get; set;}

        public string DelayValueName { get; set;}

        public float DelayValue { get; set;}

        public string StartValue { get; set; }

        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitDelay(this);
        }

        public DelayNodeModel(string name, string fulname, string input, string output, int deep, string delayName, float delayValue) : base()
        {
            this.Name = name;
            this.Id = name;
            this.InputRate = input;
            this.OutputRateName = output;
            this.FullName = fulname;
            this.DeepDelay = deep;
            this.DelayValueName = delayName;
            this.DelayValue = delayValue;


            var a = new NodeOutputViewModel();
            a.PortPosition = PortPosition.Right;
            a.Name = this.Name;
            this.Outputs.Add(a);

            a = new NodeOutputViewModel();
            a.PortPosition = PortPosition.Right;
            a.Name = this.DelayValueName;
            Outputs.Add(a);


            a = new NodeOutputViewModel();
            a.PortPosition = PortPosition.Right;
            a.Name = this.OutputRateName;
            Outputs.Add(a);


            var b = new NodeInputViewModel();
            b.PortPosition = PortPosition.Left;
            Inputs.Add(b);
        }
        public DelayNodeModel() : this("LЕV", "Запаздывание", "1", "OUT", 1, "DEL", 1) {}
        static DelayNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("level"), typeof(IViewFor<DelayNodeModel>));
        }
    
        public override JsonObject ToJSON()
        {
            JsonObject obj = new JsonObject()
            {
                ["Id"] = Id,
                ["Type"] = type,
                ["Name"] = Name == null ? "" : "Name",
                ["FullName"] = FullName == null ? "" : FullName,
                ["InputRate"] = InputRate == null ? "" : InputRate,
                ["OutputRateName"] = OutputRateName == null ? "" : OutputRateName,
                ["DeepDelay"] =  DeepDelay,
                ["DelayValueName"] = DelayValueName == null ? "" : DelayValueName,
                ["DelayValue"] = DelayValue,
                ["StartValue"] = StartValue == null ? "" : StartValue
            };

            return obj;
        }

        public override void FromJSON(JsonObject obj)
        {
            Id = obj!["Id"]!.GetValue<string>();
            Name = obj!["Name"]!.GetValue<string>();
            FullName = obj!["FullName"]!.GetValue<string>();
            InputRate = obj!["InputRate"]!.GetValue<string>();
            OutputRateName = obj!["OutputRateName"]!.GetValue<string>();
            DeepDelay = obj!["DeepDelay"]!.GetValue<int>();
            DelayValueName = obj!["DelayValueName"]!.GetValue<string>();
            DelayValue = obj!["DelayValue"]!.GetValue<float>();
            StartValue = obj!["StartValue"]!.GetValue<string>();

        }

    }

}
