using DynamicData;
using NodeNetwork.ViewModels;
using System.Windows;
using ForresterModeller.src.Nodes.Models;

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


            var node1 = new ConstantNodeViewModel();
            network.Nodes.Add(node1);

            var node2 = new LevelNodeModel();
            network.Nodes.Add(node2);

            var node3 = new FunkNodeModel();
            network.Nodes.Add(node3);


            var node4 = new CrossNodeModel();
            network.Nodes.Add(node4);

            var node5 = new ChouseNodeModel();
            network.Nodes.Add(node5);

            var node6 = new DelayNodeModel();
            network.Nodes.Add(node6);


            networkView.ViewModel = network;
            
        }

        private void networkView_Loaded()
        {

        }
    }
}
