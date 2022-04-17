
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

        public int DeepDelay { get; set;}

        public string DelayValueName { get; set;}

        public double DelayValue { get; set;}

        public string StartValue { get; set; }
        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var prop = base.GetProperties();
            prop.Add(new PropertyViewModel("Имя исходящего потока", OutputRateName, (String str) =>
            {
                OutputRateName = str;
                _outNode.Name = str;
            }));
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
            }));
            prop.Add(new PropertyViewModel("Величина запаздывания принятия решений", DelayValue.ToString(), (String str) => { DelayValue = Utils.GetDouble(str); }));
            return prop;
        }

        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitDelay(this);
        }


        public override ObservableCollection<DataForViewModels> GetMathView()
        {
            var data = base.GetMathView();
         
            foreach (var inputs in Inputs.Items)
            {
                if (inputs.Connections.Items.Count() > 0)

                {
                    String value = ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue;
                    data.Add(new DataForViewModels(inputs.Name, value, false));
                }
            }


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

            var a = new ForesterNodeOutputViewModel();
            a.PortPosition = PortPosition.Right;
            a.Name = this.Name;
            this.Outputs.Add(a);

            _constNode = new ForesterNodeOutputViewModel();
            _constNode.PortPosition = PortPosition.Right;
            _constNode.OutFunc = () => this.DelayValueName;
            _constNode.Name = this.DelayValueName;
            Outputs.Add(_constNode);


            _outNode = new ForesterNodeOutputViewModel();
            _outNode.PortPosition = PortPosition.Right;
            _outNode.OutFunc = () => this.OutputRateName;
            _outNode.Name = this.OutputRateName;
            Outputs.Add(_outNode);


            _inputRate = new NodeInputViewModel();
            _inputRate.PortPosition = PortPosition.Left;
            Inputs.Add(_inputRate);
        }
        public DelayNodeModel() : this("LЕV", "Запаздывание", "1", "OUT", 1, "DEL", 1, "0") {}
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
                ["DeepDelay"] =  DeepDelay,
                ["DelayValueName"] = DelayValueName == null ? "" : DelayValueName,
                ["DelayValue"] = DelayValue,
                ["StartValue"] = StartValue == null ? "" : StartValue
            };

            JsonArray con = new();
            if (_inputRate.Connections.Items.Any())
            {
                con.Add(new ConectionModel(_inputRate).ToJSON());
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
            FullName = obj!["FullName"]!.GetValue<string>();
            InputRate = obj!["InputRate"]!.GetValue<string>();
            OutputRateName = obj!["OutputRateName"]!.GetValue<string>();
            DeepDelay = obj!["DeepDelay"]!.GetValue<int>();
            DelayValueName = obj!["DelayValueName"]!.GetValue<string>();
            DelayValue = obj!["DelayValue"]!.GetValue<float>();
            StartValue = obj!["StartValue"]!.GetValue<string>();

        }

    }

}
