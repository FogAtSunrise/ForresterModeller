using ForresterModeller.src.Windows.ViewModels;
using System.Collections.ObjectModel;

namespace ForresterModeller.src.Interfaces
{
   public  interface IMathViewable
    {
        public abstract ObservableCollection<DataForViewModels> GetMathView();
    }
}
