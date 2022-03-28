using ForresterModeller.src.Nodes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;

namespace ForresterModeller.src.ProjectManager
{
    public class Project
    {
        string DefaultName = "New Project";
        string DefaultPath = Directory.GetCurrentDirectory();

        List<IForesterModel> allProjectModels = new List<IForesterModel>();
        List<string> listAllFiles = new List<string>();

        string Name;
        public string getName() { return Name; }
        string PathToFile;

        DateTime CreationDate;
        public DateTime getCreationDate() { return CreationDate; }
        DateTime ChangeDate;
        public DateTime getChangeDate() { return ChangeDate; }

        public Project(string name, string pathTofile)
        {
            Name = (name == null || name == "") ? DefaultName : Name;
            PathToFile = (pathTofile == null || pathTofile == "") ? DefaultPath : pathTofile;
            CreationDate = DateTime.Now;
            ChangeDate = DateTime.Now;
        }


        //привер вызова:   CreateDirectory(path + "\\" + "имя новой папки");
        private string CreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                int index = 1;
                while (Directory.Exists(path + index))
                {
                    index++;
                }
                path = path + index;
            }
            Directory.CreateDirectory(path);
            return path;
        }

        private string CreateFile(string filename, string path)
        {
            if (Directory.Exists(path + "\\" + filename + ".json"))
            {
                int index = 1;
                while (Directory.Exists(path + "\\" + filename + index + ".json"))
                {
                    index++;
                }
                filename = filename + index;
            }

        


            return filename;
        }

        public Project() { }

        public void ToJson()
        {
            string path = PathToFile;
            //Объект проекта, он один
            JsonObject obj = new JsonObject
            {

                //Информация о проекте
                ["Name"] = Name,
                ["CreationDate"] = CreationDate,
                ["ChangeDate"] = ChangeDate,

                //Список файлов проекта
                ["ListAllFiles"] = new JsonArray(),

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
            //    obj!["ModelsInProject"]![nameId] = new JsonObject { ["Id"] = 1289, ["Low"] = 20 };

            //Удалить
            // obj!["ModelsInProject"].Remove("Model2");
            //  (JsonObject)obj["ModelsInProject"].Remove("Model2");

            CreateDirectory(path + "\\" + "test");

          /*  string name = CreateFile("test", path);

            StreamWriter file = new StreamWriter(path + "\\" + name + ".json");
            file.WriteLine(obj);
            file.Close();
          */
            var options = new JsonSerializerOptions { WriteIndented = true };
            MessageBox.Show(obj.ToJsonString(options));

        }

        public IForesterModel createModel(string type)
        {

            if (LevelNodeModel.type == type)
            {
                return new LevelNodeModel();
            }
            else
              if (ChouseNodeModel.type == type)
            {
                return new ChouseNodeModel();
            }
            else
                if (ConstantNodeViewModel.type == type)
            {
                return new ConstantNodeViewModel();
            }
            else
                if (CrossNodeModel.type == type)
            {
                return new CrossNodeModel();
            }
            else
                if (FunkNodeModel.type == type)
            {
                return new FunkNodeModel();
            }
            else
                if (DelayNodeModel.type == type)
            {
                return new DelayNodeModel();
            }

            return null;
        }

    }
}
