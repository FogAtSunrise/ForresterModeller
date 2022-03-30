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
        private TabViewModel _activeTab;
        public TabViewModel ActiveTab
        {
            set { this.RaiseAndSetIfChanged(ref _activeTab, value); }
            get => _activeTab;
        }

        #region Commands

        public ReactiveCommand<TabViewModel, Unit> CloseTab { get; }
     

        #endregion

        public void AddTab(WorkAreaManager contentManager)
        {
            Tabs.Add(new TabViewModel(contentManager));
        }
        public TabControlViewModel()
        {
            CloseTab = ReactiveCommand.Create<TabViewModel>(o => Tabs.Remove(o));
            
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