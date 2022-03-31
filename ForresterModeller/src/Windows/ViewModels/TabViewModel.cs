using System;
using System.ComponentModel;
using System.Windows.Controls;
using DynamicData.Binding;
using ForresterModeller.src.ProjectManager.WorkArea;
using ReactiveUI;

namespace ForresterModeller.ViewModels
{
    public class TabViewModel : ReactiveObject
    {
        public string Header => WAManager.Name;
        public ContentControl Content => WAManager.Content;
        //содержимое
        private WorkAreaManager _wamanager;
        public WorkAreaManager WAManager { get; set; }

        public TabViewModel(WorkAreaManager workAreaManager)
        {
            WAManager = workAreaManager;
            //Биндим данные с помощью инструментов Reactive
            WAManager.WhenAnyValue(w => w.Name).ToProperty(this, o => o.Header);
            WAManager.WhenAnyValue(w => w.Content).ToProperty(this, o => o.Content);
         
        }

       }
}