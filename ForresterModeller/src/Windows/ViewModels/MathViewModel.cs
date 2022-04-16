using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.Windows.ViewModels
{
   public class MathViewModel
    {
            public MathViewModel(String name, String value, Action<String> updateAction)
            {
                Name = name;
                _value = value;
                _updateValue = updateAction;
                IsReadOnly = false;
            }
     
            public MathViewModel(String name, String value)
            {
                Name = name;
                _value = value;
                IsReadOnly = true;
            }
            public string Name { get; set; }
            private Action<String> _updateValue;
            private string _value;
            public string Value
            {
                get => _value;
                set
                {
                    _value = value;
                    _updateValue(value);
                }

            }
            public bool IsReadOnly { get; set; }
        }
}
