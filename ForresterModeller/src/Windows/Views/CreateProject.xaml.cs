using System;
using System.Reactive.Linq;
using System.Windows;
using ForresterModeller.src.Windows.ViewModels;
using ReactiveUI;

namespace ForresterModeller.src.Windows.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateProject.xaml
    /// </summary>
    public partial class CreateProject : Window, IViewFor<CreateWindowViewModel>
    {
        //public string FileName = "";


        public CreateProject(StartWindowViewModel startVM)
        {
            InitializeComponent();
            this.ViewModel = new CreateWindowViewModel(startVM);
            this.DataContext = this.ViewModel;

            this.ViewModel
                .WhenAnyValue(x => x.DialogResult)
                .Where(x => null != x)
                .Subscribe(val =>
                {
                    this.DialogResult = val;
                    this.Close();
                });
            
        }
      


        public CreateWindowViewModel ViewModel { get; set; }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CreateWindowViewModel)value;
        }


    }
}
