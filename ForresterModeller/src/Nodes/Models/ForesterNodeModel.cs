using System;
using NodeNetwork.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForresterModeller.src.Pages.Properties;

namespace ForresterModeller.src.Nodes.Models
{
    /// <summary>
    /// Базовая модель узла в схеме форестера
    /// </summary>
    public class ForesterNodeModel : NodeViewModel, IForesterModel
    {
        
        public string Description { get; set; }
        public virtual string TypeName { get; set; } 
        public string FullName { get; set; } 
        public string Id { get; set; }

        public virtual ObservableCollection<Property> GetProperties()
        {
            var properties = new ObservableCollection<Property>();
            properties.Add(new Property(Resource.name, Name, (String str) => { Name = str; }));
            properties.Add(new Property(Resource.fullName, FullName, (String str) => { FullName = str; }));
            properties.Add(new Property(Resource.type, TypeName));
            properties.Add(new Property(Resource.description, Description, (String str) => { Description = str; }));
            return properties;
        }
    }


}
