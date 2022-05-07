using System;
using NodeNetwork.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ForresterModeller.src.Nodes.Viters;
using System.Text.Json.Nodes;
using ForresterModeller.src.Interfaces;
using ReactiveUI;
using ForresterModeller.src.Windows.ViewModels;
using ForresterModeller.src.Nodes.Views;
using DynamicData;
using ForresterModeller.src.ProjectManager.miniParser;

namespace ForresterModeller.src.Nodes.Models
{
    //!!!!! Для отображения измений свойств на форме представления нужно в сеттере
    //бросать событие this.RaiseAndSetIfChanged(ref _field_name, value);

    /// <summary>
    /// Базовая модель узла в схеме форестера
    /// </summary>
    public abstract class ForesterNodeModel : NodeViewModel, IForesterModel, IMathViewable
    {
        private string _description;
        private string _full_name;
        public List<ConectionModel> DumpConections { get; set; } = new();


        public string Description
        {
            get => _description;
            set
            {
                this.RaiseAndSetIfChanged(ref _description, value);
            }
        }

        private bool _isCorrect = true;
        public bool IsCorrect
        {
            get => _isCorrect;
            set
            {
                this.RaiseAndSetIfChanged(ref _isCorrect, value);
            }
        }
        public string FullName
        {
            get => _full_name;
            set
            {
                this.RaiseAndSetIfChanged(ref _full_name, value);
            }
        }
        public virtual string TypeName { get; }

        public string Id { get; set; }

        public virtual string GetCoreCode()
        {
            return Id;
        }
        public ForesterNodeModel()
        {
            Id = "OBJ" + new Random().Next().ToString();
        }
        public abstract T AcceptViseter<T>(INodeViseters<T> viseter);
        public virtual ObservableCollection<PropertyViewModel> GetProperties()
        {
            var command = ReactiveCommand.CreateFromObservable<Unit, int>(
                _ => Observable.Return(42).Delay(TimeSpan.FromSeconds(2)));

            var properties = new ObservableCollection<PropertyViewModel>();
            properties.Add(new PropertyViewModel(Resource.name, Name, (String str) => { Name = str; }, Pars.CheckName));
            properties.Add(new PropertyViewModel(Resource.fullName, FullName, (String str) => { FullName = str; }, Pars.CheckNull));
            properties.Add(new PropertyViewModel(Resource.type, TypeName));
            properties.Add(new PropertyViewModel(Resource.description, Description, (String str) => { Description = str; }, null));
            return properties;
        }

        public virtual ObservableCollection<DataForViewModels> GetMathView()
        {
            var data = new ObservableCollection<DataForViewModels>();
            data.Add(new DataForViewModels(Name, FullName, 1));
            return data;
        }

        public virtual JsonObject ToJSON() { return new JsonObject(); }
        public virtual void FromJSON(JsonObject obj) { }

        public event IPropertyOwner.PropertySelectedEventHandler PropertySelectedEvent;
        public void OnPropertySelected(IPropertyOwner sender)
        {
            PropertySelectedEvent.Invoke(this);

        }
        public virtual void AutoConection(ForesterNetworkViewModel model)
        {
            foreach (var nw in Inputs.Items.Zip(DumpConections, (n, w) => new { node = n, conections = w }))
            {
                if (nw.conections is not null)
                {
                    if (model[nw.conections.SourceId] is not LinkNodeModel)
                    {
                        model.Connections.Add(
                            new ConnectionViewModel(model,
                                nw.node,
                                model[nw.conections.SourceId].Outputs.Items.FirstOrDefault(a => a.Name == nw.conections.PointName)
                            ));
                    }
                }
            }
        }

    }
}
