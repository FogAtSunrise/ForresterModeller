using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.Windows.ViewModels;
using ReactiveUI;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    public abstract class WorkAreaManager : ReactiveObject, IPropertyOwner
    {
        public string PathToFile { get; set; }
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public event IPropertyOwner.PropertySelectedEventHandler PropertySelectedEvent;
        public void OnPropertySelected(IPropertyOwner sender)
        {
            PropertySelectedEvent?.Invoke(sender);
        }

        private string _name;
        //Объект, поля которого отображаются в окне свойств
        public virtual IPropertyOwner ActiveOwnerItem { get; set; }
        //содержимое рабочей области
        public virtual ContentControl Content { get; set; }
        public virtual ObservableCollection<PropertyViewModel> GetProperties()
        {
            var properties = new ObservableCollection<PropertyViewModel>();
            properties.Add(new PropertyViewModel("Тип", TypeName));
            properties.Add(new PropertyViewModel("Название", Name, (string s) => Name = s));
            return properties;
        }


        public virtual string TypeName { get; }
    }
}
