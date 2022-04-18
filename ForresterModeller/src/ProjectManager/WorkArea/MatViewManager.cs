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
        public DiagramManager Diagram { get; set; }

        public MatViewManager(DiagramManager dmanager)
        {
            Diagram = dmanager;
            Name = "Матпредставление для " + Diagram.Name;
            dmanager.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(DiagramManager.Name))
                    Name = "Матпредставление для " + Diagram.Name;
            };

            var mod = dmanager.GetAllNodes;
            foreach (var mod1 in mod)
            {
                Models.Add(new MathViewModel(mod1));
            }
        }

        public override string TypeName => "Математическое представление";


        //ViewModel модели, свойства которой отображаются 
        //Биндится как SelectedItem во вью
        private IPropertyOwner _activeItem;
       
        //Сама модель, владеющая проперти
        public override IPropertyOwner ActiveOwnerItem
        {
            get
            {
                if (_activeItem == null || _activeItem == null) return this;
                return _activeItem;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _activeItem, value);
                OnPropertySelected(_activeItem);
            }
        }
        public MathView GenerateActualView()
        {
            MathView m = new MathView();
            m.DataContext = this;
            return m;
        }
      /*  public MathView GenerateActualView()
        {
            MathView m = new MathView();
            m.DataContext = this;
            return m;
        }
      */
        //свойства самого матпредставления
        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var prop = base.GetProperties();
            return prop;
        }
    }
}
