using System.Collections.ObjectModel;
using ForresterModeller.src.Interfaces;
using ReactiveUI;

namespace ForresterModeller.src.Windows.ViewModels
{
    public class PropertiesControlViewModel : ReactiveObject
    {

        public ObservableCollection<PropertyViewModel> _properties;
        public ObservableCollection<PropertyViewModel> Properties
        {
            get
            {
                _properties = ActiveItem?.GetProperties();
                return _properties;
            }
            set => this.RaiseAndSetIfChanged(ref _properties, value);
        }

        //Модель, свойства которой отображаются 
        private IPropertyOwner _activeItem;
        public IPropertyOwner ActiveItem
        {
            get => _activeItem;
            set
            {
                this.RaiseAndSetIfChanged(ref _activeItem, value);
                Properties = _activeItem?.GetProperties();
            }
        }
    }
}