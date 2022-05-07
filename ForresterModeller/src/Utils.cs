using System;
using System.Collections.Generic;
using System.Globalization;
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
                digit = Double.Parse(str, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                
            }

            return digit;
        }
    }
}
