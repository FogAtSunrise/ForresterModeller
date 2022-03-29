using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Windows.Controls;
using System.Windows.Data;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Pages.Properties;
using ReactiveUI;

namespace ForresterModeller.ViewModels
{
    public class PropertiesControlViewModel: ReactiveObject
    {
        public ObservableCollection<Property> Properties { get; set; }

        //Элементы заголовка
        public string Name { get; set; }
        public string TypeName { get; set; }

        public PropertiesControlViewModel(IPropertyChangable model)
        {
            model.WhenAnyValue(w => w.TypeName).ToProperty(this, o => o.TypeName);
            model.WhenAnyValue(w => w.Name).ToProperty(this, o => o.Name);
            Properties = model.GetProperties();
        }

        //todo  SetProperty(IPropertyChangable model)

    }
}