using ForresterModeller.src.Nodes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ForresterModeller.src.ProjectManager
{
    public class Project
    {
        string Name;
        public string getName() { return Name; }
        string PathToFile;

        DateTime CreationDate;
        public DateTime getCreationDate() { return CreationDate; }
        DateTime ChangeDate; 
        public DateTime getChangeDate() { return ChangeDate; }
        public  Project(string name)
        {
            Name = name;
            CreationDate = DateTime.Now;
            ChangeDate= DateTime.Now;
        }
        public Project() { }



        public void CreateStructJson()
        {

            //Объект проекта, он один
            JsonObject obj = new JsonObject
            {

                //Информация о проекте
                ["Name"] = "project",
                ["CreationDate"] = new DateTime(2019, 8, 1),
                ["ChangeDate"] = new DateTime(2019, 8, 1),

                //Список файлов проекта
                ["FilesInProject"] = new JsonArray("file1", "file2", "file3"),

                //Список моделей проекта
                ["ModelsInProject"] = new JsonObject
                { //Пример модели
                    ["Model1"] = new JsonObject
                    {
                        ["Id"] = 123,
                        ["Name"] = "const",
                        ["Type"] = "ConstantNodeViewModel",
                        ["Value"] = "3.456"
                    },

                    ["Model2"] = new JsonObject
                    {
                        ["Id"] = 1253,
                        ["Name"] = "const"
                    }
                }
            };

            string nameId = "3456";

            //Добавить
            obj!["ModelsInProject"]![nameId] = new JsonObject { ["Id"] = 1289, ["Low"] = 20 };

            //Удалить
            // obj!["ModelsInProject"].Remove("Model2");
            //  (JsonObject)obj["ModelsInProject"].Remove("Model2");

            StreamWriter file = File.CreateText("test.json");
            file.WriteLine(obj);
            file.Close();

            //  var options = new JsonSerializerOptions { WriteIndented = true };
            // MessageBox.Show(obj.ToJsonString(options));

        }

        public IForesterModel createModel(string type)
        {

            if (LevelNodeModel.type == type)
            {
                return new LevelNodeModel();
            }
            else
              if (LevelNodeModel.type == type)
            {
                return new LevelNodeModel();
            }
            else
                if (LevelNodeModel.type == type)
            {
                return new LevelNodeModel();
            }

            return null;
        }

    }
}
