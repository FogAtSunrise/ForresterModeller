using System;
using NodeNetwork.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForresterModeller.src.Pages.Properties;
using ForresterModeller.src.Nodes.Viters;
using System.Text.Json.Nodes;

namespace ForresterModeller.src.Nodes.Models
{
    /// <summary>
    /// Базовая модель узла в схеме форестера
    /// </summary>
    public abstract class ForesterNodeModel : NodeViewModel, IForesterModel
    {
        
        

        public string Description { get; set; }
        public virtual string TypeName { get; } 
        public string FullName { get; set; } 
        public string Id { get; set; }

        public abstract T AcceptViseter<T>(INodeViseters<T> viseter);

        public virtual ObservableCollection<Property> GetProperties()
        {
            var properties = new ObservableCollection<Property>();
            properties.Add(new Property(Resource.name, Name, (String str) => { Name = str; }));
            properties.Add(new Property(Resource.fullName, FullName, (String str) => { FullName = str; }));
            properties.Add(new Property(Resource.type, TypeName));
            properties.Add(new Property(Resource.description, Description, (String str) => { Description = str; }));
            return properties;
        }

        public virtual JsonObject ToJSON() { return new JsonObject(); }
        public virtual void FromJSON(JsonObject obj) {  }
    }



}
