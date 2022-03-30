using System;
using System.ComponentModel;
using System.Reactive;
using ForesterNodeCore;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.ProjectManager.WorkArea;
using ReactiveUI;

namespace ForresterModeller.ViewModels
{
    public class MainWindowViewModel: ReactiveObject
    {
        public TabControlViewModel TabControlVM { get; set; } = new();
        public PropertiesControlViewModel PropertiesVM { get; set; } = new();

        #region commands
        public ReactiveCommand<WorkAreaManager, Unit> OpenTabCommand { get; }

        #endregion

        public MainWindowViewModel()
        {
            OpenTabCommand = ReactiveCommand.Create<WorkAreaManager>(o => TabControlVM.AddTab(new WorkAreaManager{Name = "писбка"}));

            TabControlVM.PropertyChanged += TabControlUpdate;
        }

        private void TabControlUpdate(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TabControlVM.ActiveTab))
            {
                PropertiesVM.ActiveItem = TabControlVM.ActiveTab.WAManager;
            }
        }
        
    }
}