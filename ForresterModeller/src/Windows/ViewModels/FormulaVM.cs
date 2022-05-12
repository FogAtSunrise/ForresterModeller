using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForresterModeller.src.Nodes.Models;
using ReactiveUI;

namespace ForresterModeller.src.Windows.ViewModels
{
    public class FormulaVM:ReactiveObject
    {
        public FunkNodeModel NodeForMod { get; set; }


        public FormulaVM(FunkNodeModel node)
        {
            NodeForMod = node;
            node.PropertyChanged += (sender, args) => { Data = node?.GetFormul(); };
        }


        public DataForViewModels _data;
        public DataForViewModels Data
        {
            get
            {
                _data = NodeForMod?.GetFormul();
                return _data;
            }
            set => this.RaiseAndSetIfChanged(ref _data, value);
        }

}
}
