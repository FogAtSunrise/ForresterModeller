using System.Windows;
using System.Windows.Controls;
using WPFtest1.src.Tools;

namespace WPFtest1
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
private void Test2(object sender, RoutedEventArgs e)
        {
            Open_Page(ToolsFrame, new ToolsPage());

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

        
    }
}
