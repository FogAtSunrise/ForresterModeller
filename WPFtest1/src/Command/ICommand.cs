using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFtest1.src.Command
{
    interface ICommand
    {
        void execute();
        void undo();
    }
}
