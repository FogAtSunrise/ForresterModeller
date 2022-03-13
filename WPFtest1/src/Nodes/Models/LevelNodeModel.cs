using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using WPFtest1.src.Nodes.Views;

namespace WPFtest1.src.Nodes.Models
{
    public class LevelNodeModel : ForesterNodeModel
    {
        public string InputRate { get; set; }
        public string OutputtRate { get; set; }
        public LevelNodeModel(string name, string fulname, string input, string output) : base()
        {
            this.Name = name;
            this.FullName = fulname;
            this.Code = name;
            this.InputRate = input;
            this.OutputtRate = output;

            var a = new NodeOutputViewModel();
            a.PortPosition = PortPosition.Left;
            this.Outputs.Add(a);

            a = new NodeOutputViewModel();
            a.PortPosition = PortPosition.Left;
            Outputs.Add(a);
        }
        public LevelNodeModel() : this("LVL", "Уровень", "1", "1") { }
        static LevelNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("level"), typeof(IViewFor<LevelNodeModel>));
        }
    }


}
