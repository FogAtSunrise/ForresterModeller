using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.ProjectManager.MiniParser
{
    //isInputCorrect(xInput.Text)
    class MinParser
    {


        //Поля
        private string _formula;
        private string _inputErrorDescription;
        ProjectManager.MiniParser.Tree myTree = new ProjectManager.MiniParser.Tree();
        string t;
        private float[,] func = new float[10, 10];
        List<char> Signs = new List<char>();//список приоритета операций


        //Методы
        public double FindSolution(string t, string Formula)
        {
            this.t = t;

            double result;

            FillOperationList(); //вызов метода заполнения приоритета операций
            myTree.Root = new MathNode(); //создание корневого узла

            try
            {
                Formula = BracketDestroyer(Formula);
                myTree.Root.Val = MakeTree(Formula, myTree.Root); //вызов метода создания дерева
                result = Calculate(myTree.Root);
            }
            catch (Exception)
            {
                InputErrorDescription = "Некорректный ввод";
                throw new Exception();
            }
            return result;

        }//главный метод

        public bool isInputCorrect(string input)
        {
            if (input == "") return false;//проверка на пустоту
            int test = 0;
            //проверка скобок
            for (int i = 0; i < input.Length; i++)
            {
                if (test >= 0)
                {
                    if (input[i] == '(')
                        test++;
                    else if (input[i] == ')')
                        test--;
                }
                else
                {
                    InputErrorDescription = "Проверьте скобки";
                    throw new Exception();
                }
            }
            if (test != 0)
            {
                InputErrorDescription = "Проверьте скобки";
                throw new Exception();
            }
            else return true;


        }//проверяет корректность введенных данных

        private double Calculate(MathNode currentMathNode)
        {
            double solution = 0;
            string operation = "none";
            MathNode leftMathNode;
            MathNode rightMathNode;

            if (currentMathNode.LeftMathNode != null)
            {
                leftMathNode = new MathNode();
                leftMathNode = currentMathNode.LeftMathNode;
            }
            if (currentMathNode.RightMathNode != null)
            {
                rightMathNode = new MathNode();
                rightMathNode = currentMathNode.RightMathNode;
            }
            if (currentMathNode.IsOperator)
            {
                operation = currentMathNode.Val;
            }

            switch (operation)
            {
                case "+":
                    solution = Calculate(currentMathNode.LeftMathNode) + Calculate(currentMathNode.RightMathNode);
                    break;
                case "-":
                    solution = Calculate(currentMathNode.LeftMathNode) - Calculate(currentMathNode.RightMathNode);
                    break;
                case "^":
                    solution = Math.Pow(Calculate(currentMathNode.LeftMathNode), Calculate(currentMathNode.RightMathNode));
                    break;
                case "*":
                    solution = Calculate(currentMathNode.LeftMathNode) * Calculate(currentMathNode.RightMathNode);
                    break;
                case "/":
                    solution = Calculate(currentMathNode.LeftMathNode) / Calculate(currentMathNode.RightMathNode);
                    break;
                case "s":
                    solution = Math.Sin(Calculate(currentMathNode.RightMathNode));
                    break;
                case "c":
                    solution = Math.Cos(Calculate(currentMathNode.RightMathNode));
                    break;
                default:
                    if (currentMathNode.Val == "t")
                    {
                        currentMathNode.Val = t;
                    }
                    solution = Convert.ToDouble(currentMathNode.Val);
                    break;
            }

            return solution;

        }//расчет формулы по AST-дереву

        private string MakeTree(string formula, MathNode currentMathNode)
        {
            string leftFormula = "";
            string rightFormula = "";
            string currentMathNodeValue = "";


            ParseFormula(formula, ref leftFormula, ref rightFormula, ref currentMathNodeValue);//парсинг формулы

            if (formula.Equals(currentMathNodeValue))// расставление флагов на операторы
                currentMathNode.IsOperator = false;
            else
                currentMathNode.IsOperator = true;

            if (leftFormula != "")//рекурсивно вызываем метод ParseFormula для левой и правой части от знака
            {
                currentMathNode.LeftMathNode = new MathNode();
                currentMathNode.LeftMathNode.Val = MakeTree(leftFormula, currentMathNode.LeftMathNode);
            }
            if (rightFormula != "")
            {
                currentMathNode.RightMathNode = new MathNode();
                currentMathNode.RightMathNode.Val = MakeTree(rightFormula, currentMathNode.RightMathNode);
            }
            return currentMathNodeValue;// Возвращает операцию либо операнд
        }//составление AST-дерева

        private void ParseFormula(string formula, ref string leftPart, ref string rightPart, ref string value)
        {
            for (int j = 0; j < Signs.Count() - 2; j++)//проход по списку приоритетных операций
            {
                for (int i = 0; i < formula.Length; i++)
                {
                    if (formula[i] == Signs[j])
                    {
                        value = Signs[j] + "";
                        if (i == 0 && j == 1)//проверка на отрицательное число
                        {
                            value = "_";
                            rightPart = formula.Substring(i + 1, formula.Length - i - 1);
                            return;
                        }
                        else
                        {
                            leftPart = formula.Substring(0, i);
                            rightPart = formula.Substring(i + 1, formula.Length - i - 1);
                            return;
                        }
                    }
                }

            }

            for (int j = 5; j < Signs.Count(); j++)//проход по списку приоритетных операций
            {
                for (int i = 0; i < formula.Length; i++)
                {
                    value = Signs[j] + "";
                    if (formula[i] == Signs[j])
                    {
                        rightPart = formula.Substring(i + 1, formula.Length - i - 1);
                        return;
                    }
                }

            }
            value = formula;//если не является ни одним из операторов, считает, что это операнд
            return;
        }//парсинг формулы

        private string BracketDestroyer(string formula)
        {
            int firstPoint = 0;
            int lastPoint;
            int bracketCounter = 0;
            string rightPart;
            string leftPart;

            for (int i = 0; i < formula.Length; i++)
            {
                if (formula[i] == '(')
                    bracketCounter++;
            }


            while (bracketCounter != 0)
            {
                for (int i = 0; i < formula.Length; i++)
                {
                    if (formula[i] == ')')
                    {
                        lastPoint = i;
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (formula[j] == '(')
                            {
                                firstPoint = j;
                                break;
                            }
                        }
                        leftPart = formula.Substring(0, firstPoint);
                        if (lastPoint != formula.Length - 1)
                            rightPart = formula.Substring(lastPoint + 1, formula.Length - lastPoint - 1);
                        else
                            rightPart = "";

                        Tree bracketTree = new Tree();
                        bracketTree.Root = new MathNode();
                        bracketTree.Root.Val = MakeTree(formula.Substring(firstPoint + 1, lastPoint - firstPoint - 1), bracketTree.Root);
                        formula = leftPart + Calculate(bracketTree.Root) + rightPart;
                        break;
                    }
                }
                bracketCounter--;
            }
            return formula;
        }

        private void FillOperationList()
        {
            Signs.Add('+');
            Signs.Add('-');
            Signs.Add('*');
            Signs.Add('/');
            Signs.Add('^');
            Signs.Add('s');
            Signs.Add('c');
        }//заполнение списка приоритетов операций

        //Свойства
        public string Formula
        {
            get
            {
                return _formula;
            }

            set
            {
                _formula = value;
            }
        }
        public string InputErrorDescription
        {
            get
            {
                return _inputErrorDescription;
            }

            set
            {
                _inputErrorDescription = value;
            }
        }
    }
}
