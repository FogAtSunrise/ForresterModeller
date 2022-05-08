using NodeNetwork.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Windows;

namespace ForresterModeller.src.Nodes.Models
{
    public class ConectionModel { 
        public string SourceId { get; set; }
        public string PointName { get; set; }
        public List<Point> AddotoionalPoint { get; set; } = new();

        public ConectionModel() { }


        public ConectionModel(NodeInputViewModel rate)
        {
            var outs = rate.Connections.Items.First().Output;
            SourceId = ((ForesterNodeModel)outs.Parent).Id;
            PointName = outs.Name;
            AddotoionalPoint = rate.Connections.Items.First().AdditionalPoints.Items.ToList();
        }

    }

}
