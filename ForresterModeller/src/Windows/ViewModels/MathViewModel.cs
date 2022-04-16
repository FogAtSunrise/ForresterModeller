using ForresterModeller.src.Nodes.Models;

namespace ForresterModeller.Windows.ViewModels
{
    public class MathViewModel
    {
        public ForesterNodeModel NodeForMod { get; set; }
        public MathViewModel(ForesterNodeModel node)
        {
            NodeForMod = node;
        }
    }
}
