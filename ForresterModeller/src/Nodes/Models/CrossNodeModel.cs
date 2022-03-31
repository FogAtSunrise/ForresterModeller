using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Nodes.Viters;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.Views;
using System.Linq;
using NodeNetwork;
using System.Text.Json.Nodes;

namespace ForresterModeller.src.Nodes.Models
{
    public class CrossNodeModel : ForesterNodeModel
    {
        public static string type = "CrossNodeModel";
        public CrossNodeModel()
        {



            var input = new CrossNodeModelInputRate();

            input.PortPosition = PortPosition.Centr;
            Inputs.Add(input);


            var value = new CrossNodeModelSourceRate();

            value.PortPosition = PortPosition.Centr;
            Inputs.Add(value);

            input.Source = value;
            value.Target = input;

            var outp = new NodeOutputViewModel();
            outp.PortPosition = PortPosition.Centr;
            Outputs.Add(outp);

        }

        static CrossNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("cross"), typeof(IViewFor<CrossNodeModel>));
        }

        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitCross(this);
        }

        public override JsonObject ToJSON()
        {
            JsonObject obj = new JsonObject()
            {
                ["Id"] = Id == null ? "" : Id,
                ["Type"] = type,
                ["Name"] = Name == null ? "" : Name,
                ["FullName"] = FullName == null ? "" : FullName,
            };

            return obj;
        }

        public override void FromJSON(JsonObject obj)
        {
            Id = obj!["Id"]!.GetValue<string>();
            Name = obj!["Name"]!.GetValue<string>();
            FullName = obj!["FullName"]!.GetValue<string>();


        }
    }

    public class CrossNodeModelInputRate : NodeInputViewModel
    {

        public NodeInputViewModel Source { get; set; }
        static CrossNodeModelInputRate()
        {
            Splat.Locator.CurrentMutable.Register(() => new NodeInputView(), typeof(IViewFor<CrossNodeModelInputRate>));
        }

        public CrossNodeModelInputRate()
        {

            ConnectionValidator = con =>
            {
                var level = con.Output.Parent as LevelNodeModel;
                if (Source.Connections.Items.Count() != 0)
                {
                    level.OutputRate = ((ForesterNodeModel)Source.Connections.Items.ToList()[0].Output.Parent).Id;
                }
                return new ConnectionValidationResult(true, null);
            };
        }
    }



    public class CrossNodeModelSourceRate : NodeInputViewModel
    {

        public NodeInputViewModel Target { get; set; }
        static CrossNodeModelSourceRate()
        {
            Splat.Locator.CurrentMutable.Register(() => new NodeInputView(), typeof(IViewFor<CrossNodeModelSourceRate>));
        }

        public CrossNodeModelSourceRate()
        {

            ConnectionValidator = con =>
            {
                var level = con.Output.Parent as LevelNodeModel;
                if (Target.Connections.Items.Count() != 0)
                {
                    ((LevelNodeModel)Target.Connections.Items.ToList()[0].Output.Parent).OutputRate = ((ForesterNodeModel)con.Output.Parent).Id;
                }
                return new ConnectionValidationResult(true, null);
            };
        }
    }

}
