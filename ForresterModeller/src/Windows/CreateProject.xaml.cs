using System;
using System.Collections.Generic;
using System.Linq;
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
using ForresterModeller.Windows.ViewModels;

namespace ForresterModeller.src.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateProject.xaml
    /// </summary>
    public partial class CreateProject : Window
    {
       
        public CreateProject()
        {
            InitializeComponent();
        }


        private void OpenGuide(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path_to_project.Text = fbd.SelectedPath;
            }

        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (name_project.Text == "" || name_project.Text == null)
                name_project.BorderBrush = Brushes.Red;
            else
            if (path_to_project.Text == "" || path_to_project.Text == null)
                path_to_project.BorderBrush = Brushes.Red;
            else
            {
                path_to_project.BorderBrush = Brushes.Black;
                name_project.BorderBrush = Brushes.Black;

                //
                //Заменить на команду создания
                //
                TestClass ghg = new TestClass();
                ghg.test7(path_to_project.Text + "\\" + name_project.Text);

                this.Close();

            }
        }

       
    }
}
