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
        public WorkAreaManager WAManager { get; set; }

        public TabViewModel(WorkAreaManager WAManager)
        {
            this.WAManager = WAManager;
            //Биндим данные с помощью инструментов Reactive
            this.WAManager.WhenAnyValue(w => w.Name).ToProperty(this, o => o.Header);
            this.WAManager.WhenAnyValue(w => w.Content).ToProperty(this, o => o.Content);
        }
    }
}