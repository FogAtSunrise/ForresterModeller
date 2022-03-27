using ForresterModeller.src.Nodes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.ProjectManager
{
    class LoaderClass
    {
        public static string puth;

        public IForesterModel createModel(string type)
        {
            
   /*         switch (type)
                {
                case (LevelNodeModel.GetType().ToString()):
                    break;

            }*/
            

            return new LevelNodeModel();
        }
    }
}
