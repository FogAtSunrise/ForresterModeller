using System.Collections.ObjectModel;
using ForresterModeller.src.ProjectManager.WorkArea;
using ReactiveUI;

namespace ForresterModeller.src.Windows.ViewModels
{
    /// <summary>
    /// Биндится к TabControl
    /// </summary>
    public class TabControlViewModel : ReactiveObject
    {
        public ObservableCollection<TabViewModel> Tabs { get; } = new();

        /// <summary>
        /// Биндится к SelectedItem на View
        /// </summary>
        private TabViewModel _activeTab;
        public TabViewModel ActiveTab
        {
            set
            {
                this.RaiseAndSetIfChanged(ref _activeTab, value);
                ActiveTab?.WAManager.OnPropertySelected(ActiveTab.WAManager);
            }
            get => _activeTab;
        }

        #region Commands

        #endregion

        public void AddTabFromWAManager(WorkAreaManager contentManager)
        {
            var item = new TabViewModel(contentManager);
            Tabs.Add(item);
            ActiveTab = item;
        }
    }
}