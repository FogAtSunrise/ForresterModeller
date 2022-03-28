using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Nodes.Viters;

namespace ForresterModeller.src.Nodes.Models
{
    public class CrossNodeModel : ForesterNodeModel
    {
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
    }

}
