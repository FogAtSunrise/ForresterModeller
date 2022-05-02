using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForesterNodeCore.NodeEngine
{
    public enum NodeLiterType
    {
        Constant = 'c',
        Halt = 'h',
        Choose = 'f',
        Level = 'l',
        Delay = 'd'
    }

    public class BuilderEnine
    {
        private Dictionary<string, Node> _nodes = new();

        public BuilderEnine()
        {

        }

        public void AutoConnect()
        {
            foreach(var node in _nodes)
            {
                foreach(var symb in node.Value.FreeSymbol)
                {
                    if(_nodes.Keys.Contains(symb.ToString()))
                    {
                        this._nodes[symb.ToString()].ConectWith(node.Value, "default", symb.ToString());
                    }
                }
            }
        }

        public void MadExpDelay(string name, string nameTemp, int deep, string cons, string inputThread, float start)
        {
            this[name] = new DeepExpDelay(start, deep, inputThread, cons);
            this[nameTemp] = new Halt(name + "temp");
            this[name].ConectWith(this[nameTemp], "temp", name + "temp");
        }

        public Node this[string name]
        {
            get
            {
                return _nodes[name];
            }

            set
            {
                _nodes[name] = value;
            }
        }

        public double[] GetValue(string name, double t)
        {
            List<double> values = new();
            for(int i = 0; i < (int)(t/Node.Delta); i++)
            {
                values.Add(this[name].Value(i * Node.Delta));
            }
            return values.ToArray();
        }

        public void Translte(string text)
        {
            var nodes = text.Split('|');
            foreach(var node in nodes)
            {
                var nodeParam = node.Split();
                TranslateNode(nodeParam);
            }
            AutoConnect();
        }

        public void TranslateNode(string[] nodeParam)
        {
            switch (nodeParam[0][0])
            {
                case (char)NodeLiterType.Constant:
                    this[nodeParam[1]] = new Constant(double.Parse(nodeParam[2], CultureInfo.InvariantCulture));
                    break;
                case (char)NodeLiterType.Level:
                    this[nodeParam[1]] = new Level(double.Parse(nodeParam[2]), nodeParam[3], nodeParam[4]);
                    break;
                case (char)NodeLiterType.Halt:
                    this[nodeParam[1]] = new Halt(nodeParam[2]);
                    break;
                case (char)NodeLiterType.Choose:
                    this[nodeParam[1]] = new Choose(nodeParam[2]);
                    break;
                case (char)NodeLiterType.Delay:
                    MadExpDelay(nodeParam[1], nodeParam[2], int.Parse(nodeParam[3]), nodeParam[4], nodeParam[5], float.Parse(nodeParam[6]));
                    break;
            }
        }

        public double[][] Count(string inp, string outp, double t, double delay)
        {
            Node.Delta = delay;
            Translte(inp);
            List<double[]> result = new();
            
            foreach(var o in outp.Split())
            {
                result.Add(this.GetValue(o, t));
            }
            return result.ToArray();
        }
    }
}
