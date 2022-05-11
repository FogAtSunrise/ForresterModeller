using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Windows.ViewModels;
using System;
using System.Collections.Generic;

namespace ForresterModeller.src.ProjectManager.miniParser
{


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
            result = n;
            str = s;
        }
        public bool result;
        public string str;

    };
    public class MinParser
    {
        //крамсает строку с формулой на массив из элементов формулы
        public List<Lexem> GetFormulaArray(string t)
        {
            List<Lexem> array = new List<Lexem>();
            string t1 = t.Trim();
            if (t1.Length < 1)
                array = new List<Lexem>() { new Lexem(tEnd, "") };
            text = t + '\0';
            pointer = 0;
            Lexem lex = cutForm();
            while(lex.numb != 100)
            {
                array.Add(lex);
                lex = cutForm();
            }
            return array;
        }


        /// <summary>
        /// МЕТОД ПРИНИМАЮЩИЙ ПРОПЕРТИ И В ЗАВИСИМООСТИ ОТ ИХ ВИДА ВЫЗЫВАЮЩИЙ НУЖНУЮ ПРОВЕРКУ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Result isCorrect(String name, String value)
        {
            switch (name)
            {
                case "Уравнение":
                    return CheckFormula(value);
                    break;

                case "Обозначение":
                    return CheckName(value);


                case "Значение":
                    return CheckConst(value);


                case "Имя исходящего потока":
                    if (value == "")
                        return new Result(false, "Пустая строка");
                    else
                        return new Result(true, "");

                case "Имя велечены запаздывания":
                    if (value == "")
                        return new Result(false, "Пустая строка");
                    else
                        return new Result(true, "");

                default: return new Result(true, "");

            }
            return new Result(true, "");
        }


        //############################---ДЛЯ КОНСТАНТА---###########################################################
        public Result CheckConst(string val)
        {
            string t1 = val.Trim();
            if (t1.Length < 1)
                return new Result(false, "Введите значение");

           
                text = val + '\0';
                //разделяю формулу по запчастям, результат в  allLex
                pointer = 0;
                do
                {
                    allLex.Add(cutForm());

                    //если ошибка
                    if (allLex[allLex.Count - 1].numb == 1000)
                        return new Result(false, allLex[allLex.Count - 1].str);

                } while (allLex[allLex.Count - 1].numb != 100);

                if (allLex.Count > 2 || (allLex[0].numb != tNumb && allLex[0].numb != tId))
                    return new Result(false, "Это не константа");

                if (allLex[0].numb == tId)
                {
                    string dop = allLex[0].str;
                    ForesterNodeModel nod = MainWindowViewModel.ProjectInstance.getModelByName(dop);
                    if (nod == null || !(nod is FunkNodeModel))
                        return new Result(false, "Такой функции нет в проекте");

                }

            return new Result(true, "");
        }

        //############################---ДЛЯ ОБОЗНАЧЕНИЯ---###########################################################

        public Result CheckName(string name)
        {
            string t = name.Trim();
            if(t.Length < 1)
            return new Result(false, "Введите значение");
                ForesterNodeModel nod = MainWindowViewModel.ProjectInstance.getModelByName(name);
                if (nod != null)
                    return new Result(false, "Такое обозначение уже существует");
                else return new Result(true, "");
           

        }

        //############################---ВСЁ ДЛЯ УРАВНЕНИЙ---###########################################################
        //константы
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
        const int tAbs = 49;
        const int tMin = 50;
        const int tMax = 51;
        const int tFunc = 52;
        // Piecewise((UOF, UOF<ALF) , (ALF, True))

        const int tEnd = 100;
        const int tError = 1000;

        //переменные
        string text;
        public List<Lexem> allLex = new List<Lexem>();
        int pointer;


        /// <summary>
        /// Этот метод вызывайте для проверки уравнения
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>

        public Result CheckFormula(string t)
        {

            string t1 = t.Trim();
            if (t1.Length < 1)
                return new Result(false, "Уравнение отсутствует");

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
                return new Result(false, "Ввод не правильный, видимо, вы пропустили какой-то знак");
            }

            return new Result(true, t);
        }

       
        //##########################################################################################################
        /// <summary>
        ///методы для передвижения по лексемам
        /// </summary>
        /// <returns></returns>
        Lexem getNextLexeme()
        {
            return allLex[++pointer];
        }
        Lexem getCurrentLexeme()
        {
            return allLex[pointer];
        }
        /// <summary>
        /// рекурсивный спуск 
        /// </summary>
        /// <returns></returns>
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

            else
                if (lex.numb == tError)
            {
                pointer++;
                return lex;
            }
            else
                if (lex.numb == tAbs)
            {
                return forAbs();
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
            {
                if (lex.numb != tEnd)
                    pointer++;
                return new Lexem(tError, "Пропущен операнд");
            }

        }

        
         Lexem forAbs()
        {
            Lexem lex = getNextLexeme();
            if (lex.numb != tLScob)
                return new Lexem(tError, "Пропущена скобка");
            lex = getNextLexeme();
            if (lex.numb == tRScob)
            {
                pointer++;
                return new Lexem(tError, "Нет внутренностей");
            }
            Lexem type = checkLexeme();
            lex = getCurrentLexeme();
            if (lex.numb != tRScob)
                return new Lexem(tError, "Пропущена скобка");
            pointer++;
            return type;
        }
       

        //##########################################################################################################


        //отделяет от формулы одну лексему, написан по образу и подобию сканера у Крючковой
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



                    if (value == "abs")
                    {
                        return new Lexem(tAbs, value);
                    }
                   
                        return new Lexem(tId, value);

                }
                else
                {
                    pointer++;
                    return new Lexem(tError, "Недопустимый символ или допустимый символ в недопустимом месте!!!");

                }

            }

        }
        public string scanNumb()
        {
            string value = "";
            value += text[pointer++];
            while (Char.IsDigit(text[pointer]))
                value += text[pointer++];
            return value;
        }

    }
}

