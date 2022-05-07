using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.ProjectManager.miniParser
{
    public static class Pars
    {
        static MinParser _pars = new MinParser();
        private static void _clearAll()
        {
            _pars.allLex = new List<Lexem>();
        }
        public static Result CheckConst(string val)
        {
            _clearAll();
            return _pars.CheckConst(val);
        }
        public static Result CheckName(string name)
        {
            _clearAll();
            return _pars.CheckName(name);
        }
        public static Result CheckFormula(string t)
        {
            _clearAll();
            return _pars.CheckFormula(t);
        }  

        public static Result CheckNull(string t)
        {
            string t1 = t.Trim();
            if (t1.Length < 1)
                return new Result(false, "Введите значение");
            if(t.All(c => Char.IsLetterOrDigit(c) ||  c == '_' || c == ' '))
                return new Result(true, "");
            return new Result(false, "Лишние символы"); 
            
        }
    }
}
