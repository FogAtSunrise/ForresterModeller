using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ForesterNodeCore
{
    class Program
    {
        public static readonly string ScriptPath = "PythonScripts/main.exe";
        public static readonly string MainScriptPath = "main.exe";

        static void exequte(string args)
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

			string output = p.StandardOutput.ReadToEnd();
		}


        static void Main(string[] args)
        {
            exequte("\"c a 1 | c b 2 | l dc b a 0 | f nt dc/2 | d boo loo 1 b nt 0\" \"dc nt boo loo\" 1 0.1");
        }
    }




}
