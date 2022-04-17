using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Nodes.Viters;
using System.Text.Json.Nodes;

namespace ForresterModeller.src.Nodes.Models
{
    public class ChouseNodeModel:FunkNodeModel
    {
        public override string TypeName => Resource.chooseFuncName;
        public static string type = "ChouseNodeModel";
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

        public override JsonObject ToJSON()
        {
            var json = base.ToJSON();
            json["Type"] = type;
            return json;
        }


        static ChouseNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("chouse"), typeof(IViewFor<ChouseNodeModel>));
        }
    }
}
