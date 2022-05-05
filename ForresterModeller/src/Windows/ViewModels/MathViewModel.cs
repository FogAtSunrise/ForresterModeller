using ForresterModeller.src.Nodes.Models;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace ForresterModeller.src.Windows.ViewModels
{
    public class MathViewModel : ReactiveObject
    {
        public ForesterNodeModel NodeForMod { get; set; }
  
   
        public MathViewModel(ForesterNodeModel node)
        {
            NodeForMod = node;
            node.PropertyChanged += (sender, args) => { Data = node?.GetMathView(); };
        }

        public ObservableCollection<DataForViewModels> _data;
        public ObservableCollection<DataForViewModels> Data
        {
            get
            {
                _data = NodeForMod?.GetMathView();
                
                return _data;
            }
            set => this.RaiseAndSetIfChanged(ref _data, value);
        }
    }
}
