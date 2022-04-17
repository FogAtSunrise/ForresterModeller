using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ForresterModeller.src.ProjectManager;
using ForresterModeller.src.Windows.ViewModels;
using ForresterModeller.Windows.Views;
using ReactiveUI;

namespace ForresterModeller.src.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateProject.xaml
    /// </summary>
    public partial class CreateProject : Window, IViewFor<CreateWindowViewModel>
    {
        //public string FileName = "";


        public CreateProject()
        {
            InitializeComponent();
            this.ViewModel = new CreateWindowViewModel();
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
