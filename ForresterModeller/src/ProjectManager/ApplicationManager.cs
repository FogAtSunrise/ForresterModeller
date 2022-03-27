using ForresterModeller.src.ProjectManager.WorkArea;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Text.Json.Nodes;

using System.Windows;
using RestSharp;
using System.Text.Json;

namespace ForresterModeller.src.ProjectManager
{
    class ApplicationManager
    {
        Project activeProject;

        WorkAreaManager active;
        List<WorkAreaManager> opened;
        public void CreateStructJson()
        {
            
            JsonObject obj = new JsonObject()
            {
                ["ProjectFile"] = new JsonObject
                {
                    ["Name"] = "project",
                    ["CreationDate"] = new DateTime(2019, 8, 1),
                    ["ChangeDate"] = new DateTime(2019, 8, 1),

                }
            };


            StreamWriter file = File.CreateText("test.json");
            file.WriteLine(obj);
            file.Close();

            var options = new JsonSerializerOptions { WriteIndented = true };
            MessageBox.Show(obj.ToJsonString(options));

        }
    }
}
