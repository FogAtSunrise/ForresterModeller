using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    /// <summary>
    /// Табы со способностью к закрытию
    /// </summary>
    public class ActionTabItem
    {
        // This will be the text in the tab control
        public string Header { get; set; }
        // This will be the content of the tab control It is a UserControl whits you need to create manualy
        public ContentControl Content { get; set; }
    }
    ///Контейнер для закрывающихся табов
    /// (view model for the TabControl To bind on)
    public class ActionTabViewModal
    {
        // These Are the tabs that will be bound to the TabControl 
        public ObservableCollection<ActionTabItem> Tabs { get; set; }

        public ActionTabViewModal(TabControl tabControl)
        {
            Tabs = new ObservableCollection<ActionTabItem>();
            tabControl.ItemsSource = this.Tabs;
        }

        /// <summary>
        /// Страницы, открывающиеся при запуске по умолчанию
        /// </summary>
        public void Populate()
        {
            // Add A tab to TabControl With a specific header and Content(UserControl)
            Tabs.Add(new ActionTabItem { Header = "UserControl 1", Content = new UserControl() });
            // Add A tab to TabControl With a specific header and Content(UserControl)
            Tabs.Add(new ActionTabItem { Header = "UserControl 2", Content = new UserControl() });
        }
        /// <summary>
        /// Добавить страницу
        /// </summary>
        /// <param name="header">Заголовок страницы</param>
        /// <param name="uc">Содержимое</param>
        public void add(string header, ContentControl uc)
        {
            Tabs.Add(new ActionTabItem { Header = header, Content = uc});
        }
    }
    class Diagram:WorkAreaManager
    {
    }
}
