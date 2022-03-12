using ForresterModeller.Entities;
using ForresterModeller.src.Pages.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFtest1.src.Entities;

namespace ForresterModeller.src.Pages.Tools
{
    /// <summary>
    /// Логика взаимодействия для PlotterTools.xaml
    /// </summary>
    public partial class PlotterTools : Page
    {
        private int countColumn;
        public PlotterTools()
        {
            InitializeComponent();
            countColumn = 0;
        }

        /// <summary>
        /// вывод элементов графика из одной диаграммы, по принадлежности к классу или
        /// абстрактному классу определяет в соответствующую группу
        /// </summary>
        /// <param name="elements"></param>
        // public void ChangeListInPlotterTools(List<IDiagramEntity> elements)
        public void ChangeListInPlotterTools(List<IDiagramEntity> elements, string name)
        {
            //string name = "diagram1"+countColumn++;
            
 
            TreeViewItem treeHead = new TreeViewItem() { Header = name };
                      TreeViewItem tree1 = new TreeViewItem() { Header = "Уровни" };
                       TreeViewItem tree2 = new TreeViewItem() { Header = "Уравнения" };

                       foreach (var elem in elements)
                           if (elem is DiagramLevel)
                               tree1.Items.Add(new TreeViewItem() { Header = elem.Name});
                           else if (elem is DiagramFunction)
                               tree2.Items.Add(new TreeViewItem() { Header = elem.Name });

                       treeHead.Items.Add(tree1);
                       treeHead.Items.Add(tree2);
               
 
            
         //   TreeViewEvent.Items.Add(treeHead);

         
        }


        public List<Node> nodeList { get; set; }
        private List<Node> GetNodeList()
        {
            Node leafOneNode = new Node();
            leafOneNode.NodeName = "Leaf Node One";
            leafOneNode.Nodes = new List<Node>();

            Node leafTwoNode = new Node();
            leafTwoNode.NodeName = "Leaf Node Two";
            leafTwoNode.Nodes = new List<Node>();

            Node leafThreeNode = new Node();
            leafThreeNode.NodeName = "Leaf Node Three";
            leafThreeNode.Nodes = new List<Node>();

            Node leafOneNode1 = new Node();
            leafOneNode1.NodeName = "Leaf Node 1";
            leafOneNode1.Nodes = new List<Node>();

            Node leafTwoNode2 = new Node();
            leafTwoNode2.NodeName = "Leaf Node 2";
            leafTwoNode2.Nodes = new List<Node>();

            Node leafThreeNode3 = new Node();
            leafThreeNode3.NodeName = "Leaf Node 3";
            leafThreeNode3.Nodes = new List<Node>();


            return new List<Node>()
            {
                new Node(){NodeName="Root Node",Nodes=new List<Node>(){leafOneNode, leafTwoNode, leafThreeNode}},
                new Node(){NodeName="Root Node 1",Nodes=new List<Node>(){leafOneNode1, leafTwoNode2, leafThreeNode3}}
            };
        }
        private void ExpandTree()
        {
            if (this.TreeView_NodeList.Items != null && this.TreeView_NodeList.Items.Count > 0)
            {
                foreach (var item in this.TreeView_NodeList.Items)
                {
                    DependencyObject dependencyObject = this.TreeView_NodeList.ItemContainerGenerator.ContainerFromItem(item);
                    if (dependencyObject != null)//When the program is opened for the first time, dependencyObject is null, an error will occur
                    {
                        ((TreeViewItem)dependencyObject).ExpandSubtree();
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nodeList = GetNodeList();
            this.TreeView_NodeList.ItemsSource = nodeList;
            ExpandTree();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).Content.ToString() == "Root Node")
            {
                foreach (Node item in nodeList[0].Nodes)
                {
                    item.IsSelected = true;
                }
            }
            else if ((sender as CheckBox).Content.ToString() == "Root Node 1")
            {
                foreach (Node item in nodeList[1].Nodes)
                {
                    item.IsSelected = true;
                }
            }
            else
            {
                if (nodeList[0].Nodes.Exists(x => x.NodeName == (sender as CheckBox).Content.ToString()))
                {
                    if (nodeList[0].Nodes.All(x => x.IsSelected == true))
                    {
                        nodeList[0].IsSelected = true;
                    }
                    else
                        nodeList[0].IsSelected = null;
                }
                if (nodeList[1].Nodes.Exists(x => x.NodeName == (sender as CheckBox).Content.ToString()))
                {
                    if (nodeList[1].Nodes.All(x => x.IsSelected == true))
                    {
                        nodeList[1].IsSelected = true;
                    }
                    else
                        nodeList[1].IsSelected = null;
                }
            }
        }




        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).Content.ToString() == "Root Node")
            {
                foreach (Node item in nodeList[0].Nodes)
                {
                    item.IsSelected = false;
                }
            }
            else if ((sender as CheckBox).Content.ToString() == "Root Node 1")
            {
                foreach (Node item in nodeList[1].Nodes)
                {
                    item.IsSelected = false;
                }
            }
            else
            {
                if (nodeList[0].Nodes.Exists(x => x.NodeName == (sender as CheckBox).Content.ToString()))
                {
                    if (nodeList[0].Nodes.All(x => x.IsSelected == false))
                    {
                        nodeList[0].IsSelected = false;
                    }
                    else
                        nodeList[0].IsSelected = null;
                }
                if (nodeList[1].Nodes.Exists(x => x.NodeName == (sender as CheckBox).Content.ToString()))
                {
                    if (nodeList[1].Nodes.All(x => x.IsSelected == false))
                    {
                        nodeList[1].IsSelected = false;
                    }
                    else
                        nodeList[1].IsSelected = null;
                }
            }
        }

    }
}
