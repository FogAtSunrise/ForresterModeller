using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using ForresterModeller.src.ProjectManager.WorkArea;
using ReactiveUI;

namespace ForresterModeller.ViewModels
{
    /// <summary>
    /// Биндится к TabControl
    /// </summary>
    public class TabControlViewModel : ReactiveObject
    {
        public ObservableCollection<TabViewModel> Tabs { get; }
        /// <summary>
        /// Биндится к SelectedItem на View
        /// </summary>
        public TabViewModel ActiveTab { get; set; }
        #region Commands

        public ReactiveCommand<TabViewModel, Unit> CloseTab { get; }
        public ReactiveCommand<String, Unit> AddTab { get; }

        #endregion

        public TabControlViewModel()
        {
            CloseTab = ReactiveCommand.Create<TabViewModel>(o => Tabs.Remove(o));
            AddTab = ReactiveCommand.Create<String>(s => Tabs.Add(new TabViewModel(new WorkAreaManager { Name = s })));
            Tabs = new ObservableCollection<TabViewModel>
            {
                new TabViewModel(new DiagramManager { Name = "Very Big Title 1" }),
                new TabViewModel(new DiagramManager { Name = "Very Big Title 2" }),
                new TabViewModel(new DiagramManager { Name = "AAA" }),
                new TabViewModel(new DiagramManager { Name = "ooooooooooooo" }),
                new TabViewModel(new DiagramManager { Name = "Title 1" }),
                new TabViewModel(new DiagramManager { Name = "Title 2" }),
                new TabViewModel(new DiagramManager { Name = "Title 3" }),
                new TabViewModel(new DiagramManager { Name = "Very Big Title 3" })
            };
        }
    }
}