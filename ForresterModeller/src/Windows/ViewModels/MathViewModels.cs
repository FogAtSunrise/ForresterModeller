using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.Windows.ViewModels
{
    class MathViewModels : ReactiveObject
    {

      public ObservableCollection<MathViewModel> _models;
    public ObservableCollection<MathViewModel> Models
        {
        get
        {
            return _models;
        }
        set => this.RaiseAndSetIfChanged(ref _models, value);
    }
    
}
}
