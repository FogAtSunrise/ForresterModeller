
using ForresterModeller.src.Pages.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using ForresterModeller.src.Nodes.Models;

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
        
        public void ChangeListInPlotterTools(List<ForesterNodeModel> elements, string name)
        {
            

            Node treeHead = new Node() { NodeName = name, Nodes = new List<Node>()};
            Node tree1 = new Node() { NodeName = "Уровни",   Nodes = new List<Node>()};
            Node tree2 = new Node() { NodeName = "Уравнения", Nodes = new List<Node>()};
            foreach (var elem in elements)
                if (elem is LevelNodeModel)
                    tree1.Nodes.Add(new Node() { NodeName = elem.Name, Nodes = new List<Node>()});
                else if (elem is FunkNodeModel)
                    tree2.Nodes.Add(new Node() { NodeName = elem.Name, Nodes = new List<Node>() });
            treeHead.Nodes.Add(tree1);
            treeHead.Nodes.Add(tree2);
            nodeList = new List<Node>() { treeHead};
           // nodeList=GetNodeList();
            this.TreeView_NodeList.ItemsSource = nodeList;
          

            ExpandTree();
        }

        /// <summary>
        /// дерево элементов
        /// </summary>
        public List<Node> nodeList { get; set; }
        private List<Node> GetNodeList()
        {
            Node leafOneNode = new Node();
            leafOneNode.NodeName = "Уровень1";
            leafOneNode.Nodes = new List<Node>();

            Node leafTwoNode = new Node();
            leafTwoNode.NodeName = "Уровень2";
            leafTwoNode.Nodes = new List<Node>();

            Node leafThreeNode = new Node();
            leafThreeNode.NodeName = "Уровень3";
            leafThreeNode.Nodes = new List<Node>();

            Node leafOneNode1 = new Node();
            leafOneNode1.NodeName = "Уравнение1";
            leafOneNode1.Nodes = new List<Node>();

            Node leafTwoNode2 = new Node();
            leafTwoNode2.NodeName = "Уравнение2";
            leafTwoNode2.Nodes = new List<Node>();

            Node leafThreeNode3 = new Node();
            leafThreeNode3.NodeName = "Уравнение3";
            leafThreeNode3.Nodes = new List<Node>();


            return new List<Node>()
            {
                new Node(){NodeName="Файл1",Nodes=new List<Node>(){
                new Node(){NodeName="Уровни",Nodes=new List<Node>(){leafOneNode, leafTwoNode, leafThreeNode}},
                new Node(){NodeName="Уравнения",Nodes=new List<Node>(){leafOneNode1, leafTwoNode2, leafThreeNode3}}
                } }
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

        private Node findNode(string name, List<Node> node)
        {
            Node ans = new Node();
            foreach (Node item in node)
                if (item.NodeName == name)
                    return item;
                else if (item.Nodes != null) ans = findNode(name, item.Nodes);
                else return null;
                
            return ans;
                }

        private void checkNode(bool mean, Node node)
        {   node.IsSelected = mean;
            if(node.Nodes!=null)
            foreach (Node item in node.Nodes)
            {
                    checkNode(mean, item);
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
           Node mynode = findNode((sender as CheckBox).Content.ToString(), nodeList);
            if (mynode != null)
                checkNode(true, mynode);

       /*    else
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
       */
        }




        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Node mynode = findNode((sender as CheckBox).Content.ToString(), nodeList);
            if (mynode != null)
                checkNode(false, mynode);
            /* if ((sender as CheckBox).Content.ToString() == "Root Node")
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
             }*/

        }
    }
}
