using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ForresterModeller.src.Nodes.Models;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    class MatViewManager : WorkAreaManager
    {
     public override string TypeName => "Математическое представление";

        public IPropertyOwner ActiveModel { get; }
        public ContentControl Content { get; }
    }
}
