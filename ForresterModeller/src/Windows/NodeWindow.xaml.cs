using DynamicData;
using NodeNetwork.ViewModels;
using System.Windows;
using ForresterModeller.src.Nodes.Models;
using NodeNetwork.Toolkit.NodeList;
using ForresterModeller.src.Nodes.Viters;
using System.Linq;
using System.Windows.Controls;
using NodeNetwork.Toolkit.Layout.ForceDirected;

namespace ForresterModeller
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class NodeWindow : Window
    {
        private NetworkViewModel network;

        public NodeWindow()
        {

            InitializeComponent();
            network = new NetworkViewModel();
            network.Nodes.Add(new ConstantNodeViewModel());
            
            
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
            ForceDirectedLayouter layouter = new ForceDirectedLayouter();
            var config = new Configuration
            {
                Network = network,
            };

            layouter.Layout(config, 10000);
        }

        private void nodeList_Loaded(object sender, RoutedEventArgs e)
        {
            ForceDirectedLayouter layouter = new ForceDirectedLayouter();
            var config = new Configuration
            {
                Network = network,
            };

            layouter.Layout(config, 10000);

        }
    }
}
