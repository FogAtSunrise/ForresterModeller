using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.Windows.ViewModels
{
   public class DataForViewModels
    {

            public DataForViewModels(String name, String value, bool f)
            {
                Left = name;
                Right = value;
            sim = f ? " = " : " - ";
            formulOrNot = f;
            }
         
            public string Left { get; set; }
            public string Right { get; set; }
            
            public string sim { get; set; }
        bool formulOrNot;
   


}
}
