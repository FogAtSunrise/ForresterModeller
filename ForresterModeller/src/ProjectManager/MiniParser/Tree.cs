using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.ProjectManager.MiniParser
{
    class Tree
    {
        private MathNode _root;
        public MathNode Root
        {
            get
            {
                return _root;
            }

            set
            {
                _root = value;
            }
        }

    }
}
