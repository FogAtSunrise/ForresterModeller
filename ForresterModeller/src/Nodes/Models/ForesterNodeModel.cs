using System;
using NodeNetwork.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ForresterModeller.src.Nodes.Viters;
using System.Text.Json.Nodes;
using System.Windows.Annotations.Storage;
using ForresterModeller.Windows.ViewModels;
using ReactiveUI;

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

        public virtual string GetCoreCode()
        {
            return Id;
        }
        public ForesterNodeModel()
        {
            Id = TypeName + new Random().Next();
        }
        public abstract T AcceptViseter<T>(INodeViseters<T> viseter);

        public virtual ObservableCollection<PropertyViewModel> GetProperties()
        {
            var command = ReactiveCommand.CreateFromObservable<Unit, int>(
                _ => Observable.Return(42).Delay(TimeSpan.FromSeconds(2)));
            var properties = new ObservableCollection<PropertyViewModel>();
            properties.Add(new PropertyViewModel(Resource.name, Name, (String str) => { Name = str; }));
            properties.Add(new PropertyViewModel(Resource.fullName, FullName, (String str) => { FullName = str; }));
            properties.Add(new PropertyViewModel(Resource.type, TypeName));
            properties.Add(new PropertyViewModel(Resource.description, Description, (String str) => { Description = str; }));
            return properties;
        }
        
        public virtual JsonObject ToJSON() { return new JsonObject(); }
        public virtual void FromJSON(JsonObject obj) {  }

        public event IPropertyOwner.PropertySelectedEventHandler PropertySelectedEvent;
        public void OnPropertySelected(IPropertyOwner sender)
        {
            PropertySelectedEvent.Invoke(this);
        }


    }



}
