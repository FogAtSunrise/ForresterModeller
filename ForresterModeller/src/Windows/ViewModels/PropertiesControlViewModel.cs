using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Pages.Properties;
using ForresterModeller.src.ProjectManager.WorkArea;
using ReactiveUI;

namespace ForresterModeller.ViewModels
{
    public class PropertiesControlViewModel : ReactiveObject
    {

        public ObservableCollection<Property> _properties;
        public ObservableCollection<Property> Properties
        {
            get
            {
                _properties = ActiveItem?.GetProperties();
                return _properties;
            }
            set => this.RaiseAndSetIfChanged(ref _properties, value);
        }

        //Элементы заголовка

        private IPropertyOwner _activeItem;
        public IPropertyOwner ActiveItem
        {
            get => _activeItem;
            set
            {
                this.RaiseAndSetIfChanged(ref _activeItem, value);
                Properties = _activeItem.GetProperties();
            }
        }
 
       
    }
}