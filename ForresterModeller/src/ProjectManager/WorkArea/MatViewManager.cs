using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Pages.Properties;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    class MatViewManager : IWorkAreaManager
    {
        public ObservableCollection<Property> GetProperties()
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }
        public string PathToFile { get; set; }
        public IPropertyChangable ActiveModel { get; }
        public ContentControl Content { get; }
    }
}
