using System.Collections.ObjectModel;
using ForresterModeller.src.Windows.ViewModels;

namespace ForresterModeller.src.Interfaces
{
    public interface IPropertyOwner 
    {
        /// <summary>
        /// Вернуть перечень объектов, которые будут отражены пользователю
        /// и (если необходимо) изменены им
        /// </summary>
        /// <returns></returns>
        
        public abstract ObservableCollection<PropertyViewModel> GetProperties();
        public string TypeName { get;  }
        public string Name { get;  }

        //хранит методы подписчиков
        public delegate void PropertySelectedEventHandler(IPropertyOwner sender);

        public event PropertySelectedEventHandler PropertySelectedEvent;

        public void OnPropertySelected(IPropertyOwner sender);
    }
}