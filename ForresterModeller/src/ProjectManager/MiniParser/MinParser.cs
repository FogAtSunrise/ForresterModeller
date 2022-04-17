

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace ForresterModeller.src.ProjectManager.MiniParser
{



    class Lexem
    {
      public  Lexem(int n, string s)
        { first = n;
            second = s;
        }
       public int first;
        public string second;

    };
    public class MinParser
    {

        const int tId = 1;
        const int tNumb = 2;
        const int tPlus = 41;
        const int tMinus = 42;
        const int tMult = 43;
        const int tDiv = 44;
        const int tStep = 45;
        const int tSave = 46;
        const int tEnd = 100;


        String formul;
        string text;

        public void check(string t)
        { text = t + '\0'; }

        List<Lexem> allLex= new();

        int index = 0;
        private String getNext()
        {
            return "";
        }


        int pointer;
        int get_pointer() {
    return pointer;
}

    void put_pointer(int pointer)
    {
        pointer = pointer;
    }


        string scanNumb()
        {
            string value = "";
            value += text[pointer++];
            while (Char.IsDigit(text[pointer]))
                value += text[pointer++];
            return value;
        }
    private Lexem cutForm()
        { string value="";
            pointer = 0;
            while (true)
            {
                while (text[pointer] == ' ') 
                {
                    pointer++;
                }

                if (Char.IsDigit(text[pointer]))
                {

                    value = scanNumb();
                    if (text[pointer] == '.' && Char.IsDigit(text[pointer+1]))
                    { pointer++; value += (text[pointer] + scanNumb()); }
                    return new Lexem(tNumb, value);
                }



            }

        }

        public MinParser()
        { }

       


    }
}
