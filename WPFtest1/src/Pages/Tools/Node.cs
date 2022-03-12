using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.Pages.Tools
{
    public class Node : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        public Node()
        {

            this.isSelected1 = false;
            this.nodes = new List<Node>();
        }



        /// <summary>
        ///    Node name
        /// </summary>
        public string NodeName { get; set; }

        private readonly bool isSelected1;

        public bool GetIsSelected()
        {
            return isSelected1;
        }

        private readonly List<Node> nodes;

        public List<Node> GetNodes()
        {
            return nodes;
        }

        /// <summary>
        ///  been deleted
        /// </summary>
        private bool isSelected = false;
        public bool IsSelected
        {
            set
            {
                this.isSelected = value;
                this.NotifyPropertyChanged("IsSelected");
    }
    get { return this.isSelected; }
}



/// <summary>
///  Child node collection
/// </summary>
public List<Node> Nodes { get; set; }
    }
    public enum NodeType
{
    RootNode,//Root node
    LeafNode,//Leaf node

}
}
