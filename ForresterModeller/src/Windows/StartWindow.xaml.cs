
using ForresterModeller.src.ProjectManager;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Button = System.Windows.Controls.Button;
using Label = System.Windows.Controls.Label;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ForresterModeller
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            OpenStartList();

            ApplicationManager nnn = new ApplicationManager();
                
        }
        /// <summary>
        /// СОЗДАТЬ НОВЫЙ ПРОЕКТ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow dialog = new MainWindow();
            this.Close();
            dialog.ShowDialog();

        }
        /// <summary>
        /// ОТКРЫТЬ СУЩЕСТВУЮЩИЙ ПРОЕКТ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenOldProject_Click(object sender, RoutedEventArgs e)
        {
         /*
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Файлы json|*.json";
          
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog()==true)
            {
                System.Windows.MessageBox.Show("Выбран вот этот файл " + openFileDialog1.FileName);
                       
            }
         */
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               // System.Windows.MessageBox.Show("Выбран вот этот файл " + fbd.SelectedPath);
                Project j = new Project("", fbd.SelectedPath); 
                j.ToJson();
            }
            /*MainWindow dialog = new MainWindow();
            this.Close();
            dialog.ShowDialog();
            */

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
                System.Windows.Controls.Button but = new Button() { Name = lis.getName(), Height = 50 };
                DockPanel.SetDock(but, System.Windows.Controls.Dock.Top); 

                Canvas can = new Canvas(){ Height = Double.NaN,  Width = Double.NaN };
                Label one = new Label() { Width = Double.NaN, Content = lis.getName() };
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
