using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Nodes.Viters;
using System.Text.Json.Nodes;
using System.Linq;
using System.Reactive.Linq;
using ForresterModeller.src.Windows.ViewModels;
using System.Windows;
using ForresterModeller.src.ProjectManager.miniParser;
using ForresterModeller.src.ProjectManager.WorkArea;
using System.Text.Json;
using Microsoft.FSharp.Core;

namespace ForresterModeller.src.Nodes.Models
{
    public class FunkNodeModel : ForesterNodeModel
    {
        public static string type = "FunkNodeModel";
        private ForesterNodeOutputViewModel _out;
        public override string TypeName => Resource.funcType;
        public string Funk { get; set; }
        public FunkNodeModel(string name, string fulname, string funk) : base()
        {
            this.Name = name;
            this.Funk = funk;
            this.FullName = fulname;
            _out = new ForesterNodeOutputViewModel();
            _out.PortPosition = PortPosition.Right;
            _out.Name = name;
            this.Outputs.Add(_out);

            RefreshInput();
        }
        public FunkNodeModel() : this("FUN", "функция", "(x+y)/2") { }
        static FunkNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("funk"), typeof(IViewFor<FunkNodeModel>));
        }
        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var properties = base.GetProperties();
            properties.RemoveAt(0);

            properties.Insert(0, new PropertyViewModel(Resource.name, Name, (String str) =>
            {
                Name = str;
                Outputs.Items.First().Name = str;
            }, Pars.CheckName));


            properties.Add(new PropertyViewModel(Resource.equationType, Funk, (String str) => { Funk = str; RefreshInput(); }, Pars.CheckFormula));
            //todo парсер на поля в уравнеии и их добавление в проперти
            return properties;
        }


        public override ObservableCollection<DataForViewModels> GetMathView()
        {
            var data = base.GetMathView();
            data.Insert(0, new DataForViewModels("Где", "", 3));
         
            MinParser parser = new MinParser();
            List<Lexem> array =  parser.GetFormulaArray(Funk);

            foreach (var inputs in Inputs.Items)
            {
                if (inputs.Connections.Items.Count() > 0)

                {
                    String value = ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue;

                    ObservableCollection<DiagramManager> diagrams = MainWindowViewModel.ProjectInstance.Diagrams;
                    ForesterNodeModel node = null;
                    foreach (var diag in diagrams)
                    {

                        diag.UpdateNodes();
                        node = diag.АllNodes.FirstOrDefault(x =>
                            {
                               
                            if (x is DelayNodeModel || x is LinkNodeModel)
                            {
                                ForesterNodeModel y;
                                if (x is DelayNodeModel)
                                    y = (DelayNodeModel)x;
                                else
                                   y = (LinkNodeModel)x;

                                foreach (var outp in y.Outputs.Items)
                                {
                                    if (outp.Connections.Items.Count() > 0)
                                    {
                                        String val =
                                            ((ForesterNodeOutputViewModel)outp.Connections.Items.ToList()[0]
                                                .Output).OutputValue;
                                        if (val == value)
                                        {
                     
                                            return true;
                                        }
                                    }
                                }

                            }
                            else if (x.Id == value)
                                return true;

                            return false;
                        }
                        );
                        if (node != null)
                        {
                            data.Add(new DataForViewModels(node.Name, (node is LinkNodeModel)?"Сторонняя диаграмма":node.FullName, 1));
                            foreach (var word in array)
                            {
                                if (word.str == inputs.Name)
                                    word.str = node.Name;
                            }
                            break;
                        }
                    }
                    if (node == null)
                    {
                        data.Add(new DataForViewModels("Значение не найдено", "", 2));
                    }

                }
                else
                    data.Add(new DataForViewModels(inputs.Name, "Значение не задано", 1));
            }
            
            string funk = "";
            foreach (var word in array)
            {
                funk += word.str;
            }
            data.Insert(0, new DataForViewModels(Name, funk, 0));
            return data;
        }


        public void RefreshInput()
        {
            var vars = ForesterNodeCore.Program.GetArgs(this.Funk);
            foreach (var _var in vars)
            {
                if (this.Inputs.Items.ToList().FindAll(port => port.Name == _var).Count == 0)
                {
                    var b = new NodeInputViewModel();
                    b.Name = _var;
                    b.PortPosition = PortPosition.Left;
                    Inputs.Add(b);
                }
            }

            foreach (var _var in this.Inputs.Items.ToList())
            {
                if (!vars.ToList().Contains(_var.Name))
                {
                    Inputs.Remove(_var);
                }
            }


        }
        public override JsonObject ToJSON()
        {
            JsonObject obj = new JsonObject()
            {
                ["Id"] = Id,
                ["Type"] = type,
                ["Name"] = Name == null ? "" : Name,
                ["FullName"] = FullName == null ? "" : FullName,
                ["Funk"] = Funk == null ? "" : Funk,
                ["Description"] = Description == null ? "" : Description,
                ["PositionX"] = Position.X,
                ["PositionY"] = Position.Y
            };

            JsonArray con = new();


            foreach (var inputs in this.Inputs.Items)
            {
                if (inputs.Connections.Items.Any())
                {
                    con.Add(new ConectionModel(inputs));
                }
                else
                {
                    con.Add(null);
                }
            }
            obj.Add("Conects", con);
            return obj;
        }
        public override void FromJSON(JsonObject obj)
        {
            Id = obj!["Id"]!.GetValue<string>();
            Name = obj!["Name"]!.GetValue<string>();
            _out.Name = obj!["Name"]!.GetValue<string>();
            FullName = obj!["FullName"]!.GetValue<string>();
            Funk = obj!["Funk"]!.GetValue<string>();
            Description = obj!["Description"]!.GetValue<string>();
            RefreshInput();
            Position = new Point(obj!["PositionX"]!.GetValue<double>(), obj!["PositionY"]!.GetValue<double>());
            var conList = obj!["Conects"].AsArray();

            foreach (var con in conList)
            {
                if (con is null)
                {
                    DumpConections.Add(null);
                }
                else
                {
                    DumpConections.Add(JsonSerializer.Deserialize<ConectionModel>(con));
                }
            }
        }
        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitFunc(this);
        }
    }

    public class MaxNodeModel : ChouseNodeModel
    {
        public static string type = "ChouseNodeModel"; 
        public override string TypeName => "функция выбора максимума";

        public MaxNodeModel() : base("max", "max", "(first + second + abs(first-second))/2")
        {
            Description = "Функция выбора максимума из двух аргументов.";
        }
        static MaxNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("chouse"), typeof(IViewFor<MaxNodeModel>));
        }
        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var command = ReactiveCommand.CreateFromObservable<Unit, int>(
                _ => Observable.Return(42).Delay(TimeSpan.FromSeconds(2)));

            var properties = new ObservableCollection<PropertyViewModel>();
            properties.Add(new PropertyViewModel(Resource.name, Name, (String str) => { Name = str; }, Pars.CheckName));
            properties.Add(new PropertyViewModel(Resource.fullName, FullName, (String str) => { FullName = str; }, Pars.CheckNull));
            properties.Add(new PropertyViewModel(TypeName, TypeName));
            properties.Add(new PropertyViewModel(Resource.description, Description, (String str) => { Description = str; }, null));
            return properties;
        }
    }

    public class MinNodeModel : ChouseNodeModel
    {
        public static string type = "ChouseNodeModel";
        public override string TypeName => "функция выбора минимума";

        public MinNodeModel() : base("min", "min", "(first + second - abs(first-second))/2")
        {
            Description = "Функция выбора минимума из двух аргументов.";
        }
        static MinNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("chouse"), typeof(IViewFor<MinNodeModel>));
        }
        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            
            var command = ReactiveCommand.CreateFromObservable<Unit, int>(
                _ => Observable.Return(42).Delay(TimeSpan.FromSeconds(2)));

            var properties = new ObservableCollection<PropertyViewModel>();
            properties.Add(new PropertyViewModel(Resource.name, Name, (String str) => { Name = str; }, Pars.CheckName));
            properties.Add(new PropertyViewModel(Resource.fullName, FullName, (String str) => { FullName = str; }, Pars.CheckNull));
            properties.Add(new PropertyViewModel(TypeName, TypeName));
            properties.Add(new PropertyViewModel(Resource.description, Description, (String str) => { Description = str; }, null));
            return properties;
        }
    }


    public class JumpNodeModel : ChouseNodeModel
    {
        public static string type = "ChouseNodeModel";
        public JumpNodeModel() : base("jump", "jump", "default +" +
            "(time * (time + t + abs(time - t))/2 - time * time + 1-" +
            "abs(time * (time + t + abs(time - t))/2 - time * time - 1))/2"+
            " * (jump - default)")
        {

        }

        private int abs(double v)
        {
            throw new NotImplementedException();
        }

        static JumpNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("chouse"), typeof(IViewFor<JumpNodeModel>));
        }
    }

}

