using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using WPFtest1.src.Nodes.Views;

namespace WPFtest1.src.Nodes.Models
{
    public class FunkNodeModel : ForesterNodeModel
    {
        public string Funk { get; set; }
        public FunkNodeModel(string name, string fulname, string funk) : base()
        {
            this.Name = name;
            this.FullName = fulname;
            this.Code = name;
            this.Funk = funk;

            var a = new NodeOutputViewModel();
            a.PortPosition = PortPosition.Left;
            this.Outputs.Add(a);

            for (int i = 0; i < 5; i++)
            {
                var b = new NodeInputViewModel();
                b.PortPosition = PortPosition.Circle;
                Inputs.Add(b);
            }
        }
        public FunkNodeModel() : this("LVL", "Уровень", "1") { }
        static FunkNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("funk"), typeof(IViewFor<FunkNodeModel>));
        }
    }


}
