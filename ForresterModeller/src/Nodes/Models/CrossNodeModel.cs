﻿using DynamicData;
using NodeNetwork.ViewModels;
using ReactiveUI;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.Nodes.Viters;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.Views;
using System.Linq;
using NodeNetwork;
using System.Text.Json.Nodes;
using System.Windows;

namespace ForresterModeller.src.Nodes.Models
{
    public class CrossNodeModel : ForesterNodeModel
    {
        public static string type = "CrossNodeModel";
        public override string TypeName => "crossnode";



        private CrossNodeModelSourceRate _source;
        private CrossNodeModelInputRate _input;

        public CrossNodeModel()
        {
            _input = new CrossNodeModelInputRate();

            _input.PortPosition = PortPosition.Centr;
            Inputs.Add(_input);

            _source = new CrossNodeModelSourceRate();

            _source.PortPosition = PortPosition.Centr;
            Inputs.Add(_source);

            _input.Source = _source;
            _source.Target = _input;

            var outp = new ForesterNodeOutputViewModel();
            outp.OutFunc = () => ((ForesterNodeOutputViewModel)this._source.Connections.Items.ToList()[0].Output).OutputValue;
            outp.PortPosition = PortPosition.Centr;
            Outputs.Add(outp);
        }

        static CrossNodeModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ForesterNodeView("cross"), typeof(IViewFor<CrossNodeModel>));
        }

        public override T AcceptViseter<T>(INodeViseters<T> viseter)
        {
            return viseter.VisitCross(this);
        }



        public override JsonObject ToJSON()
        {
            JsonObject obj = new JsonObject()
            {
                ["Id"] = Id,
                ["Type"] = type,
                ["PositionX"] = Position.X,
                ["PositionY"] = Position.Y
            };

            JsonArray con = new();
            if (_input.Connections.Items.Any())
            {
                con.Add(new ConectionModel(_input));
            }
            else
            {
                con.Add(null);
            }

            if (_source.Connections.Items.Any())
            {
                con.Add(new ConectionModel(_source));
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
            Position = new Point(obj!["PositionX"]!.GetValue<double>(), obj!["PositionY"]!.GetValue<double>());
            var conList = obj!["Conects"].AsArray();

            foreach (var con in conList)
            {
                if (con is null)
                {
                    _dump_conections.Add(null);
                }
                else
                {
                    _dump_conections.Add(new ConectionModel(con!["SourceId"].GetValue<string>(), con!["PointName"].GetValue<string>())); ;
                }
            }
        }

    }


    public class CrossNodeModelInputRate : NodeInputViewModel
    {

        public NodeInputViewModel Source { get; set; }
        static CrossNodeModelInputRate()
        {
            Splat.Locator.CurrentMutable.Register(() => new NodeInputView(), typeof(IViewFor<CrossNodeModelInputRate>));
        }

        public CrossNodeModelInputRate()
        {

            ConnectionValidator = con =>
            {
                var level = con.Output.Parent as LevelNodeModel;
                if (Source.Connections.Items.Count() != 0)
                {
                    level.OutputRate = ((ForesterNodeOutputViewModel)Source.Connections.Items.ToList()[0].Output).OutputValue;
                }
                return new ConnectionValidationResult(true, null);
            };
        }
    }

    public class CrossNodeModelSourceRate : NodeInputViewModel
    {

        public NodeInputViewModel Target { get; set; }
        static CrossNodeModelSourceRate()
        {
            Splat.Locator.CurrentMutable.Register(() => new NodeInputView(), typeof(IViewFor<CrossNodeModelSourceRate>));
        }

        public CrossNodeModelSourceRate()
        {

            ConnectionValidator = con =>
            {
                var level = con.Output.Parent as LevelNodeModel;
                if (Target.Connections.Items.Count() != 0)
                {
                    ((LevelNodeModel)Target.Connections.Items.ToList()[0].Output.Parent).OutputRate = ((ForesterNodeOutputViewModel)con.Output).OutputValue;
                }
                return new ConnectionValidationResult(true, null);
            };
        }
    }

}
