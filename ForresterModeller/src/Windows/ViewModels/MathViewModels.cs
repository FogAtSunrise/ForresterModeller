using ForresterModeller.src.Nodes.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForresterModeller.Windows.ViewModels;

namespace ForresterModeller.src.Windows.ViewModels
{
    class MathViewModels : ReactiveObject
    {
        public ObservableCollection<MathViewModel> Models { get; set; } = new();

    }
}
