using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Pages.Properties;
using ReactiveUI;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    public class WorkAreaManager : ReactiveObject, IPropertyChangable
    {
        public string PathToFile { get; set; }
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
        private string _name;
        //Объект, поля которого отображаются в окне свойств
        public virtual IPropertyChangable ActiveChangableItem => this;
        //содержимое рабочей области
        public virtual ContentControl Content { get; }
        public virtual ObservableCollection<Property> GetProperties()
        {
            var properties = new ObservableCollection<Property>();
            properties.Add(new Property("Тип", TypeName));
            properties.Add(new Property("Название", Name, (string s) => Name = s));
            return properties;
        }


        public virtual string TypeName { get; }
    }
}
