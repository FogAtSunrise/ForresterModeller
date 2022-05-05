﻿using System;
using System.Collections.ObjectModel;
using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Nodes.Viters;
using System.Text.Json.Nodes;
using ForresterModeller.src.Windows.ViewModels;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using ForresterModeller.src.Windows.ViewModels;

namespace ForresterModeller.src.Nodes.Models
{


    public class LevelNodeModel : ForesterNodeModel
    {
        public override string TypeName => Resource.levelType;

        public static string type = "LevelNodeModel";
        private NodeInputViewModel _inputRate;

        public string InputRate { get; set; }
        public string OutputRate { get; set; }

        public string StartValue { get; set; }

        public ForesterNodeOutputViewModel Rate {get;set;}

        public LevelNodeModel(string name, string fulname, string input, string output) : base()
        {
            this.Name = name;
            this.InputRate = input;
            this.OutputRate = output;
            this.FullName = fulname;
            this.StartValue = "0";
            this.Description="";

            var a = new ForesterNodeOutputViewModel();
            a.Name = "Уровень";
            a.PortPosition = PortPosition.Right;
            this.Outputs.Add(a);

            a = new ForesterNodeOutputViewModel();
            a.Name = "Поток";
            a.PortPosition = PortPosition.Right;
            Outputs.Add(a);

            _inputRate = new NodeInputViewModel();
            _inputRate.Name = "Поток";
            _inputRate.PortPosition = PortPosition.Left;
            Inputs.Add(_inputRate);
        }

        public override ObservableCollection<DataForViewModels> GetMathView()
        {
            var dats = base.GetMathView();
            foreach (var inputs in Inputs.Items)
            {
                if (inputs.Connections.Items.Any())
                {
                    //todo проверить на пустой список
                    String value = ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output)
                        .OutputValue;
                    ForesterNodeModel nod = MainWindowViewModel.ProjectInstance.getModelById(value);
                    dats.Add(new DataForViewModels(inputs.Name, nod.FullName, false));
                }
            }
            return dats;
        }

        public LevelNodeModel() : this("LVL", "Уровень", "Поток", "0") { }
        static LevelNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("level"), typeof(IViewFor<LevelNodeModel>));
        }


        public override JsonObject ToJSON() {
            JsonObject obj = new JsonObject() {
                ["Id"] = Id,
                ["Type"] = type,
                ["Name"] = Name,
                ["FullName"] = FullName,
                ["InputRate"] = InputRate,
                ["OutputRate"] = OutputRate,
                ["Description"] = Description == null ? "" : Description,
                ["StartValue"] = StartValue == null ? "" : StartValue,
                ["PositionX"] = Position.X,
                ["PositionY"] = Position.Y
            };

            JsonArray con = new();
            if (_inputRate.Connections.Items.Any()) {
                con.Add(new ConectionModel(_inputRate));
            }
            else
            {
                con.Add(null);
            }

            obj.Add("Conects", con);

            return obj;
        }



        public override void FromJSON(JsonObject obj) {
            Id = obj!["Id"]!.GetValue<string>();
            Name = obj!["Name"]!.GetValue<string>();
            FullName = obj!["FullName"]!.GetValue<string>();
            InputRate = obj!["InputRate"]!.GetValue<string>();
            OutputRate = obj!["OutputRate"]!.GetValue<string>();
            Description = obj!["Description"]!.GetValue<string>();
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
                    DumpConections.Add(new ConectionModel(con!["SourceId"].GetValue<string>(), con!["PointName"].GetValue<string>())   ); ;
                }
            }
        }

        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var prop = base.GetProperties();

            prop.Add(new PropertyViewModel("Начальный уровень", StartValue.ToString(), (String str) => StartValue = str));
            return prop;
        }



        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitLevel(this);
        }
    }

}
