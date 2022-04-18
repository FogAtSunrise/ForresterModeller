using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src
{
    static class Utils
    {
        public static double GetDouble(string str)
        {
           
            double digit = 1.0;
            try
            {
                digit = Double.Parse(str);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(str + " - не дабл!");
            }

            return digit;
        }
    }
}
