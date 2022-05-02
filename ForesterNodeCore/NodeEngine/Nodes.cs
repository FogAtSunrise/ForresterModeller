using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Symbolics;
using MathNet.Numerics;

namespace ForesterNodeCore.NodeEngine
{
    public class Thread
    {
        private Node _in_node;
        private Node _out_node;
        public string type { get; private set;}
        
        public Thread(Node in_node, Node out_node, string type)
        {
            _in_node = in_node;
            _out_node = out_node;
            this.type = type;
        }

        public double Value(double t)
        {
            return _in_node.Value(t, type);
        }

    }

    public abstract class Node
    {
        public readonly SymbolicExpression tine = SymbolicExpression.Variable("t");
        public static double Delta { get; set; }


        protected Dictionary<string, Thread> _input_thrads = new();
        protected Dictionary<string, Thread> _output_thrads = new();
        public HashSet<string> FreeSymbol { get; set; } = new();

        public virtual string Plug(Thread thread, string name)
        {
            this._input_thrads[name] = thread;
            return name;
        }

        public virtual string Plug(Thread thread)
        {
            return Plug(thread, GetFreeName());
        }

        protected string GetFreeName()
        {
            var index = 0;
            while (_input_thrads.Where(a => a.ToString() == "in_" + index.ToString()).Any())
            {
                index++;
            }
            return "in_" + index.ToString();
        }

        public virtual void ConectWith(Node otherNode, string type, string name)
        {
            var thread = new Thread(this, otherNode, type);
            var letter = otherNode.Plug(thread, name);
            this._output_thrads[letter] = thread;
        }

        public virtual void ConectWith(Node otherNode, string type)
        {
            var thread = new Thread(this, otherNode, type);
            var letter = otherNode.Plug(thread);
            this._output_thrads[letter] = thread;
        }

        public void RenderAnswer(SymbolicExpression answer, double t)
        {
            foreach (var sym in this._input_thrads)
            {
                if (answer.CollectVariables().Contains(sym.Key))
                {

                    answer.Substitute(sym.Key, this._input_thrads[sym.Key].Value(t));
                }

            }
        }

        public void ParseFreeSybs(params string[] exprs){
            foreach (var expr in exprs)
            {
                foreach(var sym in SymbolicExpression.Parse(expr).CollectVariables())
                {
                    FreeSymbol.Add(sym.ToString());
                }
            }
        }

        protected Delegate CompileString(string expr)
        {
            var exp = SymbolicExpression.Parse(expr);
            var vars = new List<string> { "t" };
            vars.AddRange(exp.CollectVariables().Select(a => a.ToString()));
            return exp.Compile(vars.ToArray());
        }

        protected object[] CompileArgs(double t, string[] expr)
        {
            var objs = new List<object>();
            objs.Add(t);
            foreach (var symb in expr)
            {
                objs.Add(this._input_thrads[symb].Value(t));
            }
            return objs.ToArray();
        }

        public  abstract double Value(double t, string type = "default");
    }

    public class Halt : Node
    {
        Delegate _operation;
        public Halt(string operation) : base()
        {
            this.ParseFreeSybs(operation);
            _operation = CompileString(operation);
        }

        public override double Value(double t, string type = "default")
        {
            return (double)_operation.DynamicInvoke(CompileArgs(t, FreeSymbol.ToArray()));
        }
    }

    public class Choose : Halt
    {
        Delegate _operation;
        public Choose(string operation) : base(operation)
        {
        }
    }

    public class Constant : Node
    {
        Delegate _operation;

        private double _value;
        public Constant(double value) : base()
        {
            _value = value;
        }

        public override double Value(double t, string type = "default")
        {
            return _value;
        }
    }

    public class Level : Node {
        private double _start = 0;
        protected Delegate _inputRate;
        protected Delegate _outputRate;

        protected string[] _input_vars;
        protected string[] _outpt_vars;

        private Dictionary<int, double> _data_tabe = new();


        public Level(double start_level, string input, string output) : base()
        {
            _inputRate  = CompileString(input);
            _outputRate = CompileString(output);
            _input_vars = SymbolicExpression.Parse(input).CollectVariables().Select(a => a.ToString()).ToArray();
            _outpt_vars = SymbolicExpression.Parse(output).CollectVariables().Select(a => a.ToString()).ToArray();

            _start = start_level;
            ParseFreeSybs(input, output);
        }

        public override double Value(double t, string type = "default")
        {
            if (type == "temp")
            {
                return Temp(t);
            }
            var index = (int)Math.Round(t / Delta);
            index = index < 0 ? 0 : index;
            if(!this._data_tabe.Any())
            {
                _data_tabe[0] = _start;
            }



            if (!_data_tabe.Keys.Contains(index))
            {
                double answer = _data_tabe.Values.ToList()[^1];


                for (int i = _data_tabe.Keys.ToList()[^1] + 1; i < index + 1; i++)
                {
                    var temp = (double)_inputRate.DynamicInvoke( CompileArgs(Delta * (t - 1), _input_vars)) - (double)_outputRate.DynamicInvoke(CompileArgs(Delta * (t - 1), _outpt_vars));
                    answer += Delta * temp;
                    _data_tabe[i] = answer;
                }
            }
            return _data_tabe[index];
        }

        public double Temp(double t)
        {
            return (double)_inputRate.DynamicInvoke(CompileArgs(t, _input_vars));
        }
    }

    public class ExpDelay:Level
    {
        public ExpDelay(double start, string input_rate, string average):base(start, input_rate, "0")
        {
            ParseFreeSybs(input_rate, average);
            _outputRate = (Func<double,double>) (time => Value(time)/ this._input_thrads[average].Value(time));
        }
    }

    public class DeepExpDelay : Node
    {
        private List<ExpDelay> _expDelays = new();

        public DeepExpDelay(float start, int deep, string input_rate, string average_time) : base()
        {
            ParseFreeSybs(input_rate, average_time);

            var delay = new ExpDelay(start,input_rate,average_time);
            _expDelays.Add(delay);
            for(int i = 0; i < deep - 1; i++)
            {
                var newDelay = new ExpDelay(0, "DLE" + i.ToString(), average_time);
                delay.ConectWith(newDelay, "temp", "DLE" + i.ToString());
                delay = newDelay;
                _expDelays.Add(delay);
            }

        }

        public override double Value(double t, string type = "default")
        {
            if (type == "temp")
            {
                return _expDelays[^1].Temp(t);
            }
            return _expDelays.Select(a => a.Value(t)).Sum();
        }


        public override string Plug(Thread thread, string name)
        {
            foreach(var delay in _expDelays)
                delay.Plug(thread, name);
            return base.Plug(thread, name);
        }
    }


}
