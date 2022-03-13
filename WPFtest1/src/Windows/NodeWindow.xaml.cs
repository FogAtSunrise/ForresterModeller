using DynamicData;
using NodeNetwork.ViewModels;
using System.Windows;
using WPFtest1.src.Nodes.Models;

namespace WPFtest1
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

            //Create the node for the first node, set its name and add it to the network.
            var node1 = new NodeViewModel();
            node1.Name = "Node 1";
            network.Nodes.Add(node1);

            //Create the viewmodel for the input on the first node, set its name and add it to the node.
            var node1Input = new NodeInputViewModel();
            node1Input.Name = "Node 1 input";
            node1.Inputs.Add(node1Input);

            //Create the second node viewmodel, set its name, add it to the network and add an output in a similar fashion.
            var node2 = new NodeViewModel();
            node2.Name = "Node 2";
            network.Nodes.Add(node2);

            var node2Output = new NodeOutputViewModel();
            node2Output.Name = "Node 2 output";
            node2.Outputs.Add(node2Output);


            //var network = new NetworkViewModel();

            //var node1 = new ConstantNodeViewModel();
            //network.Nodes.Add(node1);

            //var node2 = new NodeViewModel();
            //network.Nodes.Add(node2);


            networkView.ViewModel = network;
        }

        private void networkView_Loaded()
        {

        }
    }
}
