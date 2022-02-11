using System.Windows;


namespace WPFtest1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
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
    }
}
