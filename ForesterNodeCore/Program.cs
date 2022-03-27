using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        public static readonly string ScriptPath = "PythonScripts/main.exe";
        public static readonly string MainScriptPath = "main.exe";

        public static string  Exequte(string args)
        {
			System.Diagnostics.Process p = new System.Diagnostics.Process();
			p.StartInfo = new System.Diagnostics.ProcessStartInfo(ScriptPath);
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.ErrorDialog = false;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.RedirectStandardInput = true;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.Arguments = args;
			p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			p.Start();

			return p.StandardOutput.ReadToEnd();
		}

        private static string PackageArgs(string model, IEnumerable<NodeIdentificator> order, float t, float delta)
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

        private static float[] ParseDataFromString(string data)
        {
            var parseValue = Array.ConvertAll<string, float>(data.Split(" "),float.Parse);

            return parseValue;
        }

		public static Dictionary<string,float[]> GetCurve(string model, IList<NodeIdentificator> order, float t, float delta = 0.1f)
        {
            var save_localization = System.Threading.Thread.CurrentThread.CurrentCulture;

            //скрипт работает на числах с точкой, нужно заменить запятые
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            var args = PackageArgs(model, order, t, delta);
            var result = Exequte(args);

            var dataStrings = result.Split("\r\n");


            var answer = new Dictionary<string, float[]>();

            for (int i = 0; i < order.Count; i++)
            {
                answer.Add(order[i].id, ParseDataFromString(dataStrings[i]));
            }


            System.Threading.Thread.CurrentThread.CurrentCulture = save_localization;
            return answer;
        }


        static void Main(string[] args)
        {
            var c = GetCurve(
                "c a 1 | c b 2 | l dc b a 0 | f nt dc/2 | d boo loo 1 b nt 0",
                new List<NodeIdentificator> {
                    new NodeIdentificator("dc"),    
                    new NodeIdentificator("nt"),    
                    new NodeIdentificator("boo"),    
                    new NodeIdentificator("loo"),    
                },
                1,
                0.1f
                );
        }
    }




}
