using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ForresterModeller.src.Nodes.Models;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
   public interface IWorkAreaManager : IPropertyChangable
    {
        public string Name { get; set; }
        public string PathToFile { get; set; }
        //Объект, поля которого отображаются в окне свойств
        public IPropertyChangable ActiveModel { get; }
        //содержимое рабочей области
        public ContentControl Content { get; }

    }
}
