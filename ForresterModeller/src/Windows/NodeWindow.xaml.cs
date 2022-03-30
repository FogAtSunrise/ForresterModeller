using DynamicData;
using NodeNetwork.ViewModels;
using System.Windows;
using ForresterModeller.src.Nodes.Models;
using NodeNetwork.Toolkit.NodeList;
using ForresterModeller.src.Nodes.Viters;

namespace ForresterModeller
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class NodeWindow : Window
    {
        public NodeWindow()
        {

            InitializeComponent();
            var network = new NetworkViewModel();
            networkView.ViewModel = network;
            nodeList.ShowTitle = false;
            nodeList.ShowSearch = false;
            nodeList.ShowDisplayModeSelector = false;
            var nodelistModel = new NodeListViewModel();
            nodelistModel.AddNodeType<ConstantNodeViewModel>(() => new ConstantNodeViewModel());
            nodelistModel.AddNodeType<CrossNodeModel>(() => new CrossNodeModel());
            nodelistModel.AddNodeType<LevelNodeModel>(() => new LevelNodeModel());
            nodelistModel.AddNodeType<ChouseNodeModel>(() => new ChouseNodeModel());
            nodelistModel.AddNodeType<FunkNodeModel>(() => new FunkNodeModel());
            nodelistModel.AddNodeType<DelayNodeModel>(() => new DelayNodeModel());
            nodeList.ViewModel = nodelistModel;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var a = NodeTranslator.Translate(networkView.ViewModel);
        }
    }
}
