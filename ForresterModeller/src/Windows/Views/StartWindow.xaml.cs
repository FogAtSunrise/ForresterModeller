using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ForresterModeller.src.ProjectManager;
using ForresterModeller.src.Windows.ViewModels;
using ReactiveUI;

namespace ForresterModeller.src.Windows.Views
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class StartWindow : Window, IViewFor<StartWindowViewModel>
    {
        public StartWindow()
        {
            InitializeComponent();
            this.ViewModel = new StartWindowViewModel();
            this.DataContext = this.ViewModel;
        }

        public StartWindowViewModel ViewModel { get; set; }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (StartWindowViewModel)value;
        }

        private void LastPtoject_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (((ListBox)sender).SelectedItem != null)
            {
                var selected = (Project)((ListBox)sender).SelectedItem;
                ViewModel.OpenProject(selected.FullName());
            }
        }
    }
}
