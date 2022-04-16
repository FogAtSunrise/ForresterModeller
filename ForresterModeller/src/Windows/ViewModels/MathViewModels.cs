using ForresterModeller.src.Nodes.Models;
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

        public ObservableCollection<MathViewModel> Models { get; set; } = new();

        //Модель, свойства которой отображаются 
        private MathViewModel _activeItem;
        public MathViewModel ActiveItem
        {
            get => _activeItem;
            set
            {
                this.RaiseAndSetIfChanged(ref _activeItem, value);
                Active = ActiveItem?.NodeForMod;
            }
        }

        
    }
}
