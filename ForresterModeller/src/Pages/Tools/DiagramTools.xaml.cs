using ForresterModeller.src.Nodes.Models;
using NodeNetwork.Toolkit.NodeList;
using System.Windows.Controls;


namespace ForresterModeller.src.Tools
{
    /// <summary>
    /// Логика взаимодействия для GraphElements.xaml
    /// </summary>
    public partial class GraphElements : Page
    {
        public GraphElements()
        {
            InitializeComponent();

            var nodelistModel = new NodeListViewModel();
            nodelistModel.AddNodeType<ConstantNodeViewModel>(() => new ConstantNodeViewModel());
            otherNodeList.ViewModel = nodelistModel;


            nodelistModel = new NodeListViewModel();
            nodelistModel.AddNodeType<LevelNodeModel>(() => new LevelNodeModel());
            nodelistModel.AddNodeType<DelayNodeModel>(() => new DelayNodeModel());
            levelNodeList.ViewModel = nodelistModel;



            nodelistModel = new NodeListViewModel();
            nodelistModel.AddNodeType<CrossNodeModel>(() => new CrossNodeModel());
            nodelistModel.AddNodeType<ChouseNodeModel>(() => new ChouseNodeModel());
            nodelistModel.AddNodeType<FunkNodeModel>(() => new FunkNodeModel());
            equasionNodeList.ViewModel = nodelistModel;


        }

        //ИНСТРУМЕНТЫ

        //
        //ПОТОКИ
        //
        /// <summary>
        /// Инструмент - поток "Информация"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void But_Click_tool_inf(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Инструмент - поток "Материал"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void But_Click_tool_mat(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Инструмент - поток "Заказы"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void But_Click_tool_ord(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void But_Click_tool_stock(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void But_Click_tool_source(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void But_Click_tool_const(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void But_Click_tool_help(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void But_Click_tool_another(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void But_Click_tool_delay(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void But_Click_tool_level(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void But_Click_tool_func(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
