using System.Collections.ObjectModel;
using ForresterModeller.src.Pages.Properties;
using ReactiveUI;

namespace ForresterModeller.src.Nodes.Models
{
    public interface IPropertyOwner 
    {
        /// <summary>
        /// Вернуть перечень объектов, которые будут отражены пользователю
        /// и (если необходимо) изменены им
        /// </summary>
        /// <returns></returns>
        public abstract ObservableCollection<Property> GetProperties();
        public string TypeName { get;  }
        public string Name { get;  }
     
    }
}