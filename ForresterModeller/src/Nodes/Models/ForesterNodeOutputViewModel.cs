using System;
using NodeNetwork;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;

namespace ForresterModeller.src.Nodes.Models
{
    public class ForesterNodeOutputViewModel: NodeOutputViewModel
    {
        public Func<string> OutFunc { get; set; } = null;

        public string OutputValue { get => OutFunc is null ? default_output() : OutFunc(); }

        public ForesterNodeOutputViewModel() : base() { }

        private string default_output()
        {
            return ((ForesterNodeModel)this.Parent).GetCoreCode();
        }

        static ForesterNodeOutputViewModel()
        {
            NNViewRegistrar.AddRegistration(() => new NodeOutputView(), typeof(IViewFor<ForesterNodeOutputViewModel>));
        }

    }
}
