using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Windows.ViewModels;
using ForresterModeller.src.Windows.Views;
using ForresterModeller.Windows.ViewModels;
using ReactiveUI;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    class MatViewManager : WorkAreaManager
    {

        public ObservableCollection<MathViewModel> Models { get; set; } = new();

        public MatViewManager()
        { Models = new ObservableCollection<MathViewModel> { new MathViewModel(new FunkNodeModel("fuunc", "ddd", "d")), new MathViewModel(new ConstantNodeViewModel("const", "", 23)) }; }

        public override string TypeName => "Математическое представление";

        private IPropertyOwner _activeItem;
        public override IPropertyOwner ActiveOwnerItem
        {
            get => _activeItem ?? this;
            set
            {
                this.RaiseAndSetIfChanged(ref _activeItem, value);
                OnPropertySelected(_activeItem);
            }
        }

        //Модель, свойства которой отображаются 
        private MathViewModel _active;
        public MathViewModel Active
        {
            get => _active;
            set
            {
                ActiveOwnerItem = Active?.NodeForMod;
            }
        }

        public override ContentControl Content => GenerateActualView();

        public MathView GenerateActualView()
        {
            MathView m = new MathView();
            m.DataContext = this; 
            return m;
        }


        //свойства модели
        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var prop = base.GetProperties();
            return prop;
        }
    }
}
