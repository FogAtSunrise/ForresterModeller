using ForresterModeller.src.Nodes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;

namespace ForresterModeller.src.ProjectManager
{
    class TestClass
    {

        Project proj;


        public TestClass(Project p)
        { proj = p; }

        public TestClass() { }


        public void test4(string path)
        {
    

            proj = new Project(path);
      
        }
            public void test1()
        {
            proj = new Project();
           // proj.SaveNewProject();
            proj.ToJson11();

        }
        public void test2()
        {
            proj = new Project();
         //   proj = new Project("New Project2");
            proj.addModel(new LevelNodeModel("lev1", "levelishe1", "in", "out"));
            proj.addModel(new ConstantNodeViewModel("const1", "constanta", 6.8f));
            proj.addModel(new ChouseNodeModel("chous", "comment", "34x^2+11*6x=34"));
            proj.addModel(new FunkNodeModel("chous", "comment", "34x^2+11*6x=34"));
            proj.addModel(new DelayNodeModel("lev1", "levelishe1", "in", "out", 76, "constanta", 6.8f));
           
            proj.addFiles("sss");
            proj.addFiles("sttt");
            proj.addFiles("jj");

            proj.SaveNewProject();

            /*
            List<IForesterModel> elem = new List<IForesterModel>() {
                new LevelNodeModel("lev1", "levelishe1", "in", "out"),
                new ConstantNodeViewModel("const1", "constanta", 6.8f),
                new ChouseNodeModel("chous", "comment", "34x^2+11*6x=34"),
                new FunkNodeModel("chous", "comment", "34x^2+11*6x=34"),
                new DelayNodeModel(),
            };

            foreach (var el in elem)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                MessageBox.Show(el.ToJSON().ToJsonString(options));


                JsonObject test = el.ToJSON();
                test["Name"] = "Another name";

                el.FromJSON(test);
                options = new JsonSerializerOptions { WriteIndented = true };
                MessageBox.Show(el.ToJSON().ToJsonString(options));

            }
            */

        }

        public void test3()
        {
            proj.addFiles("DDDDDDDDDDDDDDDDDDDD");
            proj.SaveOldProject();
        }
        }
}
