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
        public string Header => wamanager.Name;
        public ContentControl Content => wamanager.Content;
        //содержимое
        private WorkAreaManager wamanager;

        public TabViewModel(WorkAreaManager WAManager)
        {
            wamanager = WAManager;
            //Биндим данные с помощью инструментов Reactive
            wamanager.WhenAnyValue(w => w.Name).ToProperty(this, o => o.Header);
            wamanager.WhenAnyValue(w => w.Content).ToProperty(this, o => o.Content);
        }
    }
}