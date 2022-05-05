using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.ProjectManager.miniParser
{
    /*
MinParser b = new MinParser();
    Result ho = b.check("g+12-hj)*kj/3+kj");
foreach (Lexem h in b.allLex)
{
    Console.WriteLine(h.str);
}
Console.WriteLine("error:" + ho.str);*/

    public class Lexem
    {
        public Lexem(int n, string s)
        {
            numb = n;
            str = s;
        }
        public int numb;
        public string str;

    };

    //результат парсинга, если есть ошибка, то еще и текст ошибки возвращает
    public class Result
    {
        public Result(bool n, string s)
        {
            numb = n;
            str = s;
        }
        public bool numb;
        public string str;

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
        const int tLScob = 47;
        const int tRScob = 48;

        const int tEnd = 100;
        const int tError = 1000;


        string text;
        public List<Lexem> allLex = new List<Lexem>();
        int pointer;

        //##########################################################################################################
        public Result check(string t)
        {

            text = t + '\0';
            //разделяю формулу по запчастям, результат в  allLex
            pointer = 0;
            do
            {
                allLex.Add(cutForm());

                //если ошибка
                if (allLex[allLex.Count - 1].numb == 1000)
                    return new Result(false, allLex[allLex.Count - 1].str);

            } while (allLex[allLex.Count - 1].numb != 100);


            pointer = 0;
            //рекурсивно проверяю

            if (pointer > allLex.Count)
                return new Result(false, "Что-то пошло не по плану");
            try
            {
                Lexem res = checkLexeme();
                if (res.numb == tError)
                    return new Result(false, res.str);

            }
            catch
            {
                return new Result(false, "Что-то пошло совсем-совсем не по плану");
            }

            if (allLex[pointer].numb != tEnd)
            {
                //Console.WriteLine("error:" + allLex[pointer].str);
                return new Result(false, "Ввод максимально неправильный, даже близко");
            }

            return new Result(true, t);
        }
        //##########################################################################################################
        Lexem getNextLexeme()
        {
            return allLex[++pointer];
        }
        Lexem getCurrentLexeme()
        {
            return allLex[pointer];
        }

        Lexem checkLexeme()
        {
            Lexem type = multiplier();
            Lexem lex = getCurrentLexeme();
            while (lex.numb == tPlus || lex.numb == tMinus)
            {
                lex = getNextLexeme();
                type = multiplier();
                lex = getCurrentLexeme();
            }
            return type;
        }

        Lexem multiplier()
        {
            Lexem type = stepen();
            Lexem lex = getCurrentLexeme();
            while (lex.numb == tMult || lex.numb == tDiv)
            {
                lex = getNextLexeme();
                type = stepen();
                lex = getCurrentLexeme();
            }
            return type;
        }


        Lexem stepen()
        {
            Lexem type = elementaryExpressionAnalysis();
            Lexem lex = getCurrentLexeme();
            while (lex.numb == tStep)
            {
                lex = getNextLexeme();
                type = elementaryExpressionAnalysis();
                lex = getCurrentLexeme();
            }
            return type;
        }


        Lexem elementaryExpressionAnalysis()
        {
            Lexem lex = getCurrentLexeme();

            if (lex.numb == tNumb || lex.numb == tId)
            {
                pointer++;
                return lex;
            }


            else if (lex.numb == tLScob)
            {
                pointer++;
                Lexem type = checkLexeme();
                lex = getCurrentLexeme();
                if (lex.numb != tRScob)
                    return new Lexem(tError, "Пропущена скобка");
                pointer++;
                return type;
            }
            else
                if (lex.numb == tError)
            {
                pointer++;
                return lex;
            }
            else
            {
                if (lex.numb != tEnd)
                    pointer++;
                return new Lexem(tError, "Пропущена переменная или константа");
            }

        }










        //##########################################################################################################


        //отделяет от формулы одну лексему
        private Lexem cutForm()
        {
            string value = "";

            while (true)
            {
                while (text[pointer] == ' ')
                {
                    pointer++;
                }
                if (text[pointer] == '\0')
                {

                    return new Lexem(tEnd, "END");
                }
                else
                if (Char.IsDigit(text[pointer]))
                {

                    value = scanNumb();
                    if (text[pointer] == '.' && Char.IsDigit(text[pointer + 1]))
                    { pointer++; value += (text[pointer - 1] + scanNumb()); }
                    return new Lexem(tNumb, value);
                }
                else
                if (text[pointer] == '+')
                {
                    return new Lexem(tPlus, value += text[pointer++]);
                }
                else
                 if (text[pointer] == '/')
                {
                    return new Lexem(tDiv, value += text[pointer++]);
                }
                else
                 if (text[pointer] == '-')
                {
                    return new Lexem(tMinus, value += text[pointer++]);
                }
                else
                 if (text[pointer] == '*')
                {
                    return new Lexem(tMult, value += text[pointer++]);
                }
                else
                 if (text[pointer] == '^')
                {
                    return new Lexem(tStep, value += text[pointer++]);
                }
                /*  else
                   if (text[pointer] == '=')
                  {
                      return new Lexem(tSave, value += text[pointer++]);
                  }*/
                else
                 if (text[pointer] == '(')
                {
                    return new Lexem(tLScob, value += text[pointer++]);
                }
                else
                  if (text[pointer] == ')')
                {
                    return new Lexem(tRScob, value += text[pointer++]);
                }
                else
                if (Char.IsLetter(text[pointer]) || text[pointer] == '_')
                {
                    while ((Char.IsLetterOrDigit(text[pointer]) || text[pointer] == '_') && text[pointer] != '\0' && text[pointer] != ' ')
                        value += text[pointer++];
                    return new Lexem(tId, value);

                }
                else
                {
                    pointer++;
                    return new Lexem(tError, "Недопустимый символ или допустимый символ в недопустимом месте!!!");

                }

            }

        }
        string scanNumb()
        {
            string value = "";
            value += text[pointer++];
            while (Char.IsDigit(text[pointer]))
                value += text[pointer++];
            return value;
        }

    }
}

