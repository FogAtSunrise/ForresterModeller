using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.Windows.ViewModels
{
   public class DataForViewModels
    {

            public DataForViewModels(String name, String value, int f)
            {
                Left = name;
                Right = value;
            switch (f){
                case 0: sim = "="; break;
                case 1: sim = "-"; break;
                case 3: sim = ":"; break;
                default: sim = ""; break;
            }

        }
         
            public string Left { get; set; }
            public string Right { get; set; }
            
            public string sim { get; set; }
      
   


}
}
