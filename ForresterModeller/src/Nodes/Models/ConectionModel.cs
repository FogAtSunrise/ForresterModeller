using NodeNetwork.ViewModels;
using System.Linq;
using System.Text.Json.Nodes;

namespace ForresterModeller.src.Nodes.Models
{
    public class ConectionModel{
        public string SourceId { get; set; }
        public string PointName { get; set; }

        public ConectionModel(string id, string name)
        {
            SourceId = id;
            PointName = name;
        }

        public ConectionModel(NodeInputViewModel rate)
        {
            var outs = rate.Connections.Items.First().Output;

            SourceId = ((ForesterNodeModel)outs.Parent).Id;
            PointName = outs.Name;
        }

    }

}
