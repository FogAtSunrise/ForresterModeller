using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.Command
{
    interface ICommand
    {
        void execute();
        void undo();
    }
}
