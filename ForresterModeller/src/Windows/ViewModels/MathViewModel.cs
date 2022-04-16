using ForresterModeller.src.Nodes.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.Windows.ViewModels
{
   public class MathViewModel 
    {

        public ForesterNodeModel NodeForMod { get; set; }
        public MathViewModel(ForesterNodeModel node)
            {
            NodeForMod = node;
            _value = "opisanie";
            
          
        }

        public string Name => NodeForMod.Name;
            private string _value;
            public string Value
            {
                get => _value;
                set
                {
                    _value = value;
                }

            }

        
        }
}
