using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using DynamicData;
using ForresterModeller.Entities;
using ForresterModeller.src.Pages;
using ForresterModeller.src.Pages.Properties;
using ForresterModeller.src.Pages.Tools;
using ForresterModeller.src.Tools;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using System.Windows.Media;
using WPFtest1.src.Nodes.Models;
using Brushes = System.Windows.Media.Brushes;
using ScottPlot.Plottable;
using ScottPlot;




namespace ForresterModeller
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string V = "AliceBlue";

        //  public Frame mainFrame { get; set; }
        public MainWindow()
        {
            InitializeComponent();
           
            var network = new NetworkViewModel();
            var node1 = new ConstantNodeViewModel();
            network.Nodes.Add(node1);
            var node2 = new LevelNodeModel();
            network.Nodes.Add(node2);
            networkView.ViewModel = network;


            OpenProperty();
        }

        private void OpenProperty()
        {
            Open_Page(PropertyFrame, new ConstantProperty());
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
           // openNewPage("file_name");
            // LeftBelowFrame.NavigationService.Navigate(new LeftBelow.GraphElements());
            Open_Page(ToolsFrame, new GraphElements());
        }
        //фрейм plottertools
        private void Test2(object sender, RoutedEventArgs e)
        {

            PlotterTools t = new PlotterTools();

            List<IDiagramEntity> test = new List<IDiagramEntity>();
            for (int i = 0; i < 6; i++)
                test.Add(new DiagramConstant() { Name = "константа" + i });

            for (int i = 0; i < 6; i++)
                t.ChangeListInPlotterTools(test, "name" + i);

            Open_Page(ToolsFrame, t);
            ChangeListInFileManager(new List<string> { "file1", "file2", "file3" }, "project1");

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
            TreeFiles.Items.Clear();

               TreeViewItem treeHead = new TreeViewItem() { Header = name };
            foreach (var elem in elements)
            {
                TreeViewItem treeItem = new TreeViewItem();
                treeItem.Header = elem;
               // treeItem.MouseLeftButtonUp += OpenFile_MouseLeftButtonUp;
                treeItem.MouseDoubleClick+= OpenFile_MouseLeftButtonUp;
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
          //  MessageBox.Show("Должен открыться " + item.Header);
            openNewPage((string)item.Header, "plotter");

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            OpenPages.Items.RemoveAt(OpenPages.SelectedIndex);
            
        }

        public void FillPlot(WpfPlot WpfPlot1)
        {
            InitializeComponent();
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            double[] dataX2 = { 1, 2, 3, 4, 5 };
            double[] dataY2 = { 1, 6, 11, 19, 10 };
            var a = new ScatterPlot(dataX, dataY);
            WpfPlot1.Plot.AddScatter(dataX, dataY, System.Drawing.Color.Aqua, Single.Epsilon, Single.Epsilon, MarkerShape.asterisk, LineStyle.Solid, "DUR");
            WpfPlot1.Plot.AddScatter(dataX2, dataY2, System.Drawing.Color.Blue, Single.Epsilon, Single.Epsilon, MarkerShape.asterisk, LineStyle.Solid, "DHR");
            WpfPlot1.Plot.YLabel("Объем товара (единицы)");
            WpfPlot1.Plot.XLabel("Время (недели)");
            WpfPlot1.Plot.Legend();
            WpfPlot1.Refresh();
        }

        public void FillDiagram(NetworkView diag)
        {
           
            var network = new NetworkViewModel();
            var node1 = new ConstantNodeViewModel();
            network.Nodes.Add(node1);
            var node2 = new LevelNodeModel();
            network.Nodes.Add(node2);
            diag.ViewModel = network;
        }

        /*
         *     <Grid>
        <nodenetwork:NetworkView x:Name="networkView" Background="AliceBlue"/>
    </Grid>
         * */
        private void openNewPage(string name, string type)
        {
           int  W = 130;
           int  H = 20;

           // networkView.ViewModel = network;
            TabItem page = new TabItem();
            page.Header = name;
            page.Width = W;
            page.Height = H;
            page.Header = new Canvas ();

            Canvas canvas = new Canvas() { Height = 20, Width = 120};

            TextBlock text = new TextBlock() { Text = name,  Width = 100 };
            Canvas.SetLeft(text, 0);
            Canvas.SetBottom(text, 2);
            canvas.Children.Add(text);

            Button but = new Button() { Content = "x", Height = 18, Width = 20  };
            but.Click += ButtonBase_OnClick;
            Canvas.SetRight(but, 0);
            canvas.Children.Add(but);

            page.Header = canvas;

            if (type == "diagram")
            {
                NetworkView graf = new NetworkView() {Background = Brushes.AliceBlue };


                FillDiagram(graf);
                page.Content = graf;
            }
            else if (type == "plotter")
            {

                WpfPlot plot = new WpfPlot() { Name = "WpfPlot1" };
                FillPlot(plot);
                page.Content = plot;
            }
            OpenPages.Items.Add(page);
           


        }

        private void TestPlot(object sender, RoutedEventArgs e)
        {
            openNewPage("file2", "plotter");
        }
        private void TestGraf(object sender, RoutedEventArgs e)
        {
            //  Open_Page(ToolsFrame, new PlotPage());

            openNewPage("file1", "diagram");
        }
    }
}
