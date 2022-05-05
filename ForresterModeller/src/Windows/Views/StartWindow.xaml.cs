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

     
            OpenStartList();
        }

        public StartWindowViewModel ViewModel { get; set; }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (StartWindowViewModel)value;
        }


        /// <summary>
        /// СОЗДАТЬ НОВЫЙ ПРОЕКТ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewProject_Click(object sender, RoutedEventArgs e)
        {
            
            CreateProject proj = new CreateProject();
           
            if (proj.ShowDialog() == true)
            {
               // MainWindow dialog = new MainWindow(proj.FileName);
                this.Close();
               

            }
        }
        /// <summary>
        /// ОТКРЫТЬ СУЩЕСТВУЮЩИЙ ПРОЕКТ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenOldProject_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Файлы json|*.json";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog()==true)
            {
            MainWindow dialog = new MainWindow(openFileDialog1.FileName);
           this.Close();
           dialog.ShowDialog();
            }
        }
        

        private void OpenOle_Click(object sender, RoutedEventArgs e)
        {
            
            TestClass f = new TestClass();
            f.test2();////////////////////
            System.Windows.MessageBox.Show("Сохранился тестовый проект в папке по умолчанию ");
            
        }

        public void OpenStartList()
        {
           
            List<Project> list = new List<Project>() { new Project("project1", Directory.GetCurrentDirectory()),
                new Project("project2", Directory.GetCurrentDirectory()),
                new Project("project3", Directory.GetCurrentDirectory()),
                new Project("project4", Directory.GetCurrentDirectory()),
                new Project("project5", Directory.GetCurrentDirectory()),
                new Project("project6", Directory.GetCurrentDirectory())};
      

        foreach(var lis in list)
        {
                System.Windows.Controls.Button but = new Button() { Name = lis.Name, Height = 50 };
                DockPanel.SetDock(but, System.Windows.Controls.Dock.Top); 

                Canvas can = new Canvas(){ Height = Double.NaN,  Width = Double.NaN };
                Label one = new Label() { Width = Double.NaN, Content = lis.Name };
                Canvas.SetTop(one, -13);
                Canvas.SetLeft(one, -273);

                Label two = new Label() { Width = Double.NaN, Content = lis.getCreationDate()};
                Canvas.SetTop(two, -13);
                Canvas.SetLeft(two, 94);

                Label free = new Label() { Width = Double.NaN, Content = lis.getChangeDate() };
                Canvas.SetTop(free, -13);
                Canvas.SetLeft(free, 181);

                can.Children.Add(one);
                can.Children.Add(two);
                can.Children.Add(free);

                but.Content = can;

                LastFilesPanel.Children.Add(but);
            }
    }

        private void OpenOldProject_C(object sender, RoutedEventArgs e)
        {
            MainWindow dialog = new MainWindow();
            this.Close();
            dialog.ShowDialog();
        }


    }
}
