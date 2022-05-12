using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForresterModeller.src.Nodes.Models;
namespace ForresterModeller.src.Nodes.Views
{
    public class ForesterNetworkViewModel : NetworkViewModel
    {


        public ForesterNodeModel this[string index]
        {
            get
            {
                foreach(var node in Nodes.Items)
                {
                    if (((ForesterNodeModel)node).Id == index)
                    {
                        return (ForesterNodeModel)node;
                    }
                }
                return null;
            }
        }

        public void AutoConect()
        {
            List<LinkNodeModel> lastConection = new();

            foreach (var node in Nodes.Items)
            {
  
                 ((ForesterNodeModel)node).AutoConection(this);

            }
        }
    }
}
