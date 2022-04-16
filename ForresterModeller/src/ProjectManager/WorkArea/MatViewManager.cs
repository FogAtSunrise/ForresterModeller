using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Windows.Views;
using ForresterModeller.Windows.ViewModels;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    class MatViewManager : WorkAreaManager
    {
     public override string TypeName => "Математическое представление";

        public IPropertyOwner ActiveModel { get; }
        public override ContentControl Content => GenerateActualView();

        public MathView GenerateActualView()
        {

            return new MathView();
        }


        //свойства модели
        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var prop = base.GetProperties();
            return prop;
        }
    }
}
