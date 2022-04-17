using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.ProjectManager.MiniParser
{
   public class MathNode
    {
        //Поля
        private MathNode _leftMathNode;
        private MathNode _rightMathNode;
        private string _val;
        private bool _isOperator;

        //Методы
        public bool isSheet()
        {
            return (LeftMathNode != null || RightMathNode != null);
        }//проверка на то, что узел является листом дерева.

        //Свойства
        public string Val
        {
            get
            {
                return _val;
            }

            set
            {
                _val = value;
            }
        }
        public MathNode RightMathNode
        {
            get
            {
                return _rightMathNode;
            }

            set
            {
                _rightMathNode = value;
            }
        }
        public MathNode LeftMathNode
        {
            get
            {
                return _leftMathNode;
            }

            set
            {
                _leftMathNode = value;
            }
        }
        public bool IsOperator
        {
            get
            {
                return _isOperator;
            }

            set
            {
                _isOperator = value;
            }
        }
    }
}
