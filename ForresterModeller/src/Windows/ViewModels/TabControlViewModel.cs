using System.Collections.ObjectModel;
using System.Linq;
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

        public TabViewModel AddTabFromWAManager(WorkAreaManager contentManager)
        {
            var tab = Tabs.FirstOrDefault((x) => x.WAManager == contentManager);
            if (tab == null)
            {
                tab = new TabViewModel(contentManager);
                Tabs.Add(tab);
            }
            ActiveTab = tab;
            return tab;
        }
    }
}