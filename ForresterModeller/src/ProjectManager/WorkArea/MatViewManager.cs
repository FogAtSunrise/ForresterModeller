using System.Collections.ObjectModel;
using System.Windows.Controls;
using ForresterModeller.src.Interfaces;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Windows.ViewModels;
using ForresterModeller.src.Windows.Views;
using ReactiveUI;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    class MatViewManager : WorkAreaManager
    {
        public ObservableCollection<MathViewModel> Models { get; set; } = new();
        public override ContentControl Content => GenerateActualView();

        public MatViewManager()
        { 
          //  Models = new ObservableCollection<MathViewModel> { new MathViewModel(new FunkNodeModel("fuunc", "ddd", "d")), new MathViewModel(new ConstantNodeViewModel("conssst", "safdsfd", 23)) }; 
        }

        public override string TypeName => "Математическое представление";

        //ViewModel модели, свойства которой отображаются 
        //Биндится как SelectedItem во вью
        private MathViewModel _activeItem;
        public MathViewModel Active
        {
            get => _activeItem;
            set
            {
                _activeItem = value;
                ActiveOwnerItem = Active.NodeForMod;
                
            }
        }
        //Сама модель, владеющая проперти
        private IPropertyOwner _currectProperty;
        public override IPropertyOwner ActiveOwnerItem
        {
            get
            {
                if (_activeItem == null || _activeItem.NodeForMod == null) return this;
                return _activeItem.NodeForMod;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _currectProperty, value);
                OnPropertySelected(_currectProperty);
            }
        }

        public MathView GenerateActualView()
        {
            MathView m = new MathView();
            m.DataContext = this;
            return m;
        }

        //свойства самого матпредставления
        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var prop = base.GetProperties();
            return prop;
        }
    }
}
