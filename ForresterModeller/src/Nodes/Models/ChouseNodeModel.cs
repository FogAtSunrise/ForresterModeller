using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Nodes.Viters;

namespace ForresterModeller.src.Nodes.Models
{
    public class ChouseNodeModel:FunkNodeModel
    {
        public ChouseNodeModel(string name, string fulname, string funk):base(name, fulname, funk)
        {

        }

        public ChouseNodeModel() : base()
        {

        }

        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitChoose(this);
        }

        static ChouseNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("chouse"), typeof(IViewFor<ChouseNodeModel>));
        }
    }
}
