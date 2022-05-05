using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ForresterModeller.src.ProjectManager;
using ForresterModeller.src.Windows.ViewModels;
using ForresterModeller.Windows.Views;
using ReactiveUI;
using Button = System.Windows.Controls.Button;
using Label = System.Windows.Controls.Label;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

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
    }
}
