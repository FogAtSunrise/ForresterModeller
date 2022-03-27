
using ForresterModeller.src.ProjectManager;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
        }
        /// <summary>
        /// СОЗДАТЬ НОВЫЙ ПРОЕКТ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewProject_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// ОТКРЫТЬ СУЩЕСТВУЮЩИЙ ПРОЕКТ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenOldProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow dialog = new MainWindow();
            this.Close();
            dialog.ShowDialog();

        }


        /*
         * 
          <DockPanel x:Name="LastFilesPanel">

                        <Button x:Name ="Project1" DockPanel.Dock="Top" Height="50" >
                            <Canvas Height="Auto" Width="Auto" >
                                <Label x:Name="NameProject" Canvas.Top="-13" Canvas.Left="-273"  Width="Auto " Content="Project1"/>
                                <Label x:Name="FirstDate" Canvas.Top="-13" Canvas.Left="94"  Width="Auto " Content="12.12.20"/>
                                <Label x:Name="SecondDate" Canvas.Top="-13" Canvas.Left="181"  Width="Auto " Content="26.12.20"/>
                            </Canvas>

                        </Button>
         
         */

        public void OpenStartList()
        {
            List<Project> list = new List<Project>() { new Project("project1"),
                new Project("project2"),
                new Project("project3"),
                new Project("project4"),
                new Project("project5"),
                new Project("project6")};
      

        foreach(var lis in list)
        {
                Button but = new Button() { Name = lis.getName(), Height = 50 };
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
    }
}
