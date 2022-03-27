using System;
using System.Collections.ObjectModel;
using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Pages.Properties;

namespace ForresterModeller.src.Nodes.Models
{
    public class LevelNodeModel : ForesterNodeModel
    {
        public override string TypeName
        {
            get => Resource.levelType;
            set {}
        }
        public string InputRate { get; set; }
        public string OutputRate { get; set; }
        public LevelNodeModel(string name, string fulname, string input, string output) : base()
        {
            this.Name = name;
            this.Id = name;
            this.InputRate = input;
            this.OutputRate = output;
            this.FullName = fulname;

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
    }


}
