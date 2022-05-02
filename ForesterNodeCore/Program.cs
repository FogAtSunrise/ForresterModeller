
using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace ForesterNodeCore
{
	public struct NodeIdentificator
    {
        public string name;
        public string id;

        public NodeIdentificator(string name, string id)
        {
            this.name = name;
            this.id = id;
        }

        public NodeIdentificator(string name):this(name,name)
        {

        }

    } 

    public class Program
    {


        private static string PackageArgs(string model, IEnumerable<NodeIdentificator> order, double t, double delta)
        {
            string args = "";
            args += "\"" + model + "\" \"";
            foreach(var node in order)
            {
                args += node.name + " ";
            }
            args += "\" " + t.ToString() + " " + delta.ToString();
            return args;
        }


		public static Dictionary<string,double[]> GetCurve(string model, IList<NodeIdentificator> order, double t, double delta = 0.1f)
        {
            



            var engine = new NodeEngine.BuilderEnine();
            var raw = engine.Count(model, String.Join(" ", order.Select(a => a.id)), t, delta);



            var answer = new Dictionary<string, double[]>();

            for (int i = 0; i < order.Count; i++)
            {
                answer.Add(order[i].id, raw[i]);
            }

            return answer;
        }

        public static string[] GetArgs(string aq)
        {
            var args = aq.Split(
                '(', ')', '+', '-', '/', '*', ' ', '.', ',',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '>', '=', '<', '!', '?', ':', ' '
                ).ToList();
            args = args.Distinct().ToList();
            args.Remove("sin");
            args.Remove("min");
            args.Remove("max");
            args.Remove("cos");
            args.Remove("abs");
            args.Remove("ln");
            args.Remove("");
            args.Remove(" ");
            args.Remove("t");
            return args.ToArray();
        }




        static void Main(string[] args)
        {
            //MathNet.Symbolics.SymbolicExpression.Parse("(f + s + abs(f-s))/2");

            var c = GetCurve(
                    "c a 1|c b 2|l dc 0 b a|f nt dc/2|d boo loo 1 b nt 0",
                    new List<NodeIdentificator> {
                                    new NodeIdentificator("boo"),
                    },
                    1,
                    0.1f
                    );

        }
    }




}
