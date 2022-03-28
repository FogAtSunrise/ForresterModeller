using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ForresterModeller.src.Nodes.Models;
using ReactiveUI;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    /// <summary>
    /// Табы со способностью к закрытию
    /// </summary>
    public class ActionTabItem:ReactiveObject
    {
        // This will be the text in the tab control
        public string Header => wamanager.Name;
        // This will be the content of the tab control It is a UserControl whits you need to create manualy
        //public ContentControl Content => ContentModel.Content;
        public ContentControl Content => wamanager.Content;
        //содержимое
        public WorkAreaManager wamanager;

        public ActionTabItem(WorkAreaManager WAManager)
        {
            wamanager = WAManager;
            wamanager.PropertyChanged += delegate(object? sender, PropertyChangedEventArgs args) { if(args.PropertyName == "Name") this.RaisePropertyChanged("Header"); };
        }
    }

    ///Контейнер для закрывающихся табов
    /// (view model for the TabControl To bind on)
    public class ActionTabViewModal
    {
        public TabControl tc;

        // These Are the tabs that will be bound to the TabControl 
        public ObservableCollection<ActionTabItem> Tabs { get; set; }

        public ActionTabViewModal(TabControl tabControl)
        {
            Tabs = new ObservableCollection<ActionTabItem>();
            tabControl.ItemsSource = this.Tabs;
            tc = tabControl;
        }

        /// <summary>
        /// Страницы, открывающиеся при запуске по умолчанию
        /// </summary>
        public void Populate()
        {
            // Add A tab to TabControl With a specific header and Content(UserControl)
    /*        Tabs.Add(new ActionTabItem { Header = "UserControl 1", Content = new UserControl() });
            // Add A tab to TabControl With a specific header and Content(UserControl)
            Tabs.Add(new ActionTabItem { Header = "UserControl 2", Content = new UserControl() });*/
        }
      
        /// Добавить страницу
        /// </summary>
        /// <param name="header">Заголовок страницы</param>
        /// <param name="uc">Содержимое</param>
        public void add(WorkAreaManager uc)
        {
            Tabs.Add(new ActionTabItem(uc));
        }

        public void RemoveSelected()
        {
            Tabs.RemoveAt(tc.SelectedIndex);
        }
    }
}

