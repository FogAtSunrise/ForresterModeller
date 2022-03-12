using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ForresterModeller.Entities;
using ForresterModeller.src.Tools;

namespace ForresterModeller
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      //  public Frame mainFrame { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
         //   mainFrame.NavigationService.Navigate(new Page1()); // Перемещение на страницу

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Test1(object sender, RoutedEventArgs e)
        {
           // LeftBelowFrame.NavigationService.Navigate(new LeftBelow.GraphElements());
            Open_Page(ToolsFrame, new GraphElements());
        }
        //фрейм plottertools
private void Test2(object sender, RoutedEventArgs e)
        {

            ToolsPage t = new ToolsPage();

            List<IDiagramEntity> test = new List<IDiagramEntity>();
            for (int i = 0; i < 6; i++)
                test.Add(new DiagramConstant() { Name = "константа"+i });

            for (int i = 0; i < 6; i++)
            t.ChangeListInPlotterTools(test, "name"+i);
         
            Open_Page(ToolsFrame, t);


            ChangeListInFileManager(new List<string> {"file1","file2","file3" }, "project1");

        }
        /// <summary>
        /// ОТКРЫТЬ УКАЗАННУЮ СТРАНИЦУ, В УКАЗАННОМ ФРЕЙМЕ
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="page"></param>
        private void Open_Page(Frame frame, Page page)
        {
            frame.NavigationService.Navigate(page);
        }



        /// <summary>
        /// вывод файлов проекта
        /// </summary>
        /// elements - список имен файлов
        /// name - имяпроекта
       
        public void ChangeListInFileManager(List<string> elements, string name)
        {
            TreeViewItem treeHead = new TreeViewItem() { Header = name };
            foreach (var elem in elements)
            {
                TreeViewItem treeItem = new TreeViewItem();
                treeItem.Header = elem;
                treeItem.MouseLeftButtonUp += OpenFile_MouseLeftButtonUp;
                treeHead.Items.Add(treeItem);

            }
            TreeFiles.Items.Add(treeHead);

        }


        /// <summary>
        /// Ловит, какой узел был нажат в дереве, тобишь определяет,
        /// какой файл проекта пользователь хочет открыть через файловый менеджер 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OpenFile_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            MessageBox.Show("Должен открыться "+item.Header);
         
        }

    }
}
