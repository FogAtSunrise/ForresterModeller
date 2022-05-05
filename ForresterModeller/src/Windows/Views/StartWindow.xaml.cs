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

            if (openFileDialog1.ShowDialog() == true)
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

       
        private void OpenOldProject_C(object sender, RoutedEventArgs e)
        {
            MainWindow dialog = new MainWindow();
            this.Close();
            dialog.ShowDialog();
        }
    }
}
