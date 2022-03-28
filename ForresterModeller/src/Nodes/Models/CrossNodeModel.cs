using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Nodes.Viters;
using System.Text.Json.Nodes;

namespace ForresterModeller.src.Nodes.Models
{
    public class CrossNodeModel : ForesterNodeModel
    {
        public static string type = "ChouseNodeModel";
        public CrossNodeModel()
        {



            var input = new NodeInputViewModel();

            input.PortPosition = PortPosition.Centr;
            Inputs.Add(input);


            var value = new NodeInputViewModel();

            value.PortPosition = PortPosition.Centr;
            Inputs.Add(value);



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

}
