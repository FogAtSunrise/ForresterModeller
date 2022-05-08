using System;
using System.Collections.ObjectModel;
using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Nodes.Viters;
using System.Text.Json.Nodes;
using ForresterModeller.src.Windows.ViewModels;
using System.Linq;
using System.Windows;
using ForresterModeller.src.ProjectManager.miniParser;
using ForresterModeller.src.ProjectManager.WorkArea;
using System.Text.Json;

namespace ForresterModeller.src.Nodes.Models
{
    public class DelayNodeModel : ForesterNodeModel
    {
        public static string type = "DelayNodeModel";
        private ForesterNodeOutputViewModel _constNode;
        private ForesterNodeOutputViewModel _outNode;
        private NodeInputViewModel _inputRate;

        public override string TypeName => Resource.levelType;

        public string InputRate { get; set; }

        public string OutputRateName { get; set; }

        public int DeepDelay { get; set; }

        public string DelayValueName { get; set; }

        public double DelayValue { get; set; }

        public string StartValue { get; set; }

        private ForesterNodeOutputViewModel _levl;

        private Result _check(string Name)
        {
            var r = Pars.CheckName(Name);
            if (!r.result)
                return r;
            if (Name == _levl.Name || Name == _outNode.Name || Name == DelayValueName)
            {
                r.result = false;
                r.str = "Имя не уникально!";
            }

            return r;
        }

        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var prop = base.GetProperties();
            prop.RemoveAt(0);

            prop.Insert(0, new PropertyViewModel(Resource.name, Name, (String str) =>
            {
                Name = str;
                _levl.Name = str;
            }, _check));


            prop.Add(new PropertyViewModel("Имя исходящего потока", OutputRateName, (String str) =>
            {
                OutputRateName = str;
                _outNode.Name = str;
            }, _check));

            prop.Add(new PropertyViewModel("Имя велечены запаздывания", DelayValueName, (String str) =>
            {
                DelayValueName = str;
                _constNode.Name = str;
            }, _check));
            prop.Add(new PropertyViewModel("Начальный уровень", StartValue.ToString(), (String str) => StartValue = str, Pars.CheckConst));
            prop.Add(new PropertyViewModel("Глубина запаздывания", DeepDelay.ToString(), (String str) =>
            {
                try
                {
                    DeepDelay = int.Parse(str);
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show("Только целочисленные значения");
                }
            }, Pars.CheckConst));
            prop.Add(new PropertyViewModel("Величина запаздывания принятия решений", DelayValue.ToString(),
                (String str) => { DelayValue = Utils.GetDouble(str); }, Pars.CheckConst));
            return prop;
        }

        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitDelay(this);
        }

        public override ObservableCollection<DataForViewModels> GetMathView()
        {
            var data = base.GetMathView();
            data.Add(new DataForViewModels("Входной поток", "", 3));

            foreach (var inputs in Inputs.Items)
            {
                if (inputs.Connections.Items.Count() > 0)

                {
                    String value = ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue;

                    ObservableCollection<DiagramManager> diagrams = MainWindowViewModel.ProjectInstance.Diagrams;

                    foreach (var diag in diagrams)
                    {

                        diag.UpdateNodes();
                        var node = diag.АllNodes.FirstOrDefault(x =>
                        {
                            if (x is DelayNodeModel)
                            {
                                DelayNodeModel y = (DelayNodeModel)x;
                                foreach (var outp in y.Outputs.Items)
                                {
                                    if (outp.Connections.Items.Count() > 0)
                                    {
                                        String val = ((ForesterNodeOutputViewModel)outp.Connections.Items.ToList()[0].Output).OutputValue;
                                        if (val == value)
                                        {
                                            value = ((ForesterNodeOutputViewModel)outp.Connections.Items.ToList()[0].Output).Name;
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
                            data.Add(new DataForViewModels(node.Name + (node is DelayNodeModel ? " (порт " + value + ")" : ""), node.FullName, 1));
                    }
                }
            }
            /*foreach (var inputs in Inputs.Items)
            {
                if (inputs.Connections.Items.Any())

                {
                    //todo проверить на пустой список
                    String value = ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue;

                    ForesterNodeModel nod = MainWindowViewModel.ProjectInstance.getModelById(value);
                    if(nod!= null)
                      data.Add(new DataForViewModels(inputs.Name, nod.FullName, 1));
                }
            }
            */

            return data;
        }

        public DelayNodeModel(string name, string fulname, string input, string output, int deep, string delayName, float delayValue, string startValue) : base()
        {
            this.Name = name;
            this.InputRate = input;
            this.OutputRateName = output;
            this.FullName = fulname;
            this.DeepDelay = deep;
            this.DelayValueName = delayName;
            this.DelayValue = delayValue;
            this.StartValue = startValue;

            _levl = new ForesterNodeOutputViewModel();
            _levl.PortPosition = PortPosition.Right;
            _levl.Name = this.Name;
            this.Outputs.Add(_levl);

            _constNode = new ForesterNodeOutputViewModel();
            _constNode.PortPosition = PortPosition.Right;
            _constNode.OutFunc = () => this.GetCoreCode() + '_' + this.DelayValueName;
            _constNode.Name = this.DelayValueName;
            Outputs.Add(_constNode);


            _outNode = new ForesterNodeOutputViewModel();
            _outNode.PortPosition = PortPosition.Right;
            _outNode.OutFunc = () => this.GetCoreCode() + '_' + this.OutputRateName;
            _outNode.Name = this.OutputRateName;
            Outputs.Add(_outNode);


            _inputRate = new NodeInputViewModel();
            _inputRate.PortPosition = PortPosition.Left;
            _inputRate.Name = "Поток";

            Inputs.Add(_inputRate);
        }
        public DelayNodeModel() : this("LЕV", "Запаздывание", "Поток", "OUT", 1, "DEL", 1, "0") { }
        static DelayNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("level"), typeof(IViewFor<DelayNodeModel>));
        }

        public override JsonObject ToJSON()
        {
            JsonObject obj = new JsonObject()
            {
                ["Id"] = Id,
                ["Type"] = type,
                ["Name"] = Name == null ? "" : Name,
                ["FullName"] = FullName == null ? "" : FullName,
                ["InputRate"] = InputRate == null ? "" : InputRate,
                ["OutputRateName"] = OutputRateName == null ? "" : OutputRateName,
                ["DeepDelay"] = DeepDelay,
                ["DelayValueName"] = DelayValueName == null ? "" : DelayValueName,
                ["DelayValue"] = DelayValue,
                ["StartValue"] = StartValue == null ? "" : StartValue,
                ["PositionX"] = Position.X,
                ["PositionY"] = Position.Y
            };

            JsonArray con = new();
            if (_inputRate.Connections.Items.Any())
            {
                con.Add(new ConectionModel(_inputRate));
            }
            else
            {
                con.Add(null);
            }

            obj.Add("Conects", con);

            return obj;
        }

        public override void FromJSON(JsonObject obj)
        {
            Id = obj!["Id"]!.GetValue<string>();
            Name = obj!["Name"]!.GetValue<string>();
            _levl.Name = Name;
            FullName = obj!["FullName"]!.GetValue<string>();
            InputRate = obj!["InputRate"]!.GetValue<string>();
            OutputRateName = obj!["OutputRateName"]!.GetValue<string>();
            _outNode.Name = OutputRateName;
            DeepDelay = obj!["DeepDelay"]!.GetValue<int>();
            DelayValueName = obj!["DelayValueName"]!.GetValue<string>();
            _constNode.Name = DelayValueName;
            DelayValue = obj!["DelayValue"]!.GetValue<float>();
            StartValue = obj!["StartValue"]!.GetValue<string>();
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
                    DumpConections.Add(JsonSerializer.Deserialize<ConectionModel>(con)) ;
                }
            }

        }

    }

}
