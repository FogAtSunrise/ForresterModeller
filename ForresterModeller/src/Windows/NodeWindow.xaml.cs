using DynamicData;
using NodeNetwork.ViewModels;
using System.Windows;
using ForresterModeller.src.Nodes.Models;
using NodeNetwork.Toolkit.NodeList;
using ForresterModeller.src.Nodes.Viters;
using System.Linq;
using System.Windows.Controls;
using NodeNetwork.Toolkit.Layout.ForceDirected;
using NodeNetwork.Toolkit.Group;
using System.Collections.Generic;

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
            var grouper = new BoopaG
            {
                GroupNodeFactory = subnet => new NodeViewModel { Name = "Group" },
                EntranceNodeFactory = () => new NodeViewModel { Name = "Entrance" },
                ExitNodeFactory = () => new NodeViewModel { Name = "Exit" },
                SubNetworkFactory = () => new NetworkViewModel(),
                IOBindingFactory = (groupNode, entranceNode, exitNode) =>
                    new BoopaGrouper(groupNode, entranceNode, exitNode)
            };

            grouper.EmptyGroup(network, network.Nodes.Items.ToList());
        }


        class BoopaG : NodeGrouper
        {
            public BoopaGrouper EmptyGroup(NetworkViewModel network, IEnumerable<NodeViewModel> nodesToGroup)
            {
                BoopaGrouper gr = (BoopaGrouper)this.MergeIntoGroup(network, nodesToGroup);
                foreach(var node in nodesToGroup)
                {
                    foreach(var inp in node.Inputs.Items)
                    {
                        if(inp.Connections.Count == 0)
                        {
                            gr.AddLink(inp);
                        }
                    }

                    foreach (var inp in node.Outputs.Items)
                    {
                        if (inp.Connections.Count == 0)
                        {
                            gr.AddLink(inp);
                        }
                    }
                }
                return gr;
            }
        }




        class BoopaGrouper : NodeGroupIOBinding
        {
            public BoopaGrouper(NodeViewModel groupNode, NodeViewModel entranceNode, NodeViewModel exitNode) : base( groupNode,  entranceNode,  exitNode) {
            }


            public void AddLink(NodeInputViewModel inp)
            {
                this.GroupNode.Inputs.Add(inp);
            }

            public void AddLink(NodeOutputViewModel inp)
            {
                this.GroupNode.Outputs.Add(inp);
            }


            public override NodeInputViewModel AddNewGroupNodeInput(NodeOutputViewModel candidateOutput)
            {
                throw new System.NotImplementedException();
            }

            public override NodeOutputViewModel AddNewGroupNodeOutput(NodeInputViewModel candidateInput)
            {
                throw new System.NotImplementedException();
            }

            public override NodeOutputViewModel AddNewSubnetInlet(NodeInputViewModel candidateInput)
            {
                throw new System.NotImplementedException();
            }

            public override NodeInputViewModel AddNewSubnetOutlet(NodeOutputViewModel candidateOutput)
            {
                throw new System.NotImplementedException();
            }

            public override NodeInputViewModel GetGroupNodeInput(NodeOutputViewModel subnetInlet)
            {
                throw new System.NotImplementedException();
            }

            public override NodeOutputViewModel GetGroupNodeOutput(NodeInputViewModel subnetOutlet)
            {
                throw new System.NotImplementedException();
            }

            public override NodeOutputViewModel GetSubnetInlet(NodeInputViewModel entranceInput)
            {
                throw new System.NotImplementedException();
            }

            public override NodeInputViewModel GetSubnetOutlet(NodeOutputViewModel groupNodeOutput)
            {
                throw new System.NotImplementedException();
            }
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
