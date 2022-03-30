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


        //пример вызова:   CreateDirectory(path + "\\" + "имя новой папки");
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
            if (File.Exists(path + "\\" + filename + ".json"))
            {
                int index = 1;
                while (File.Exists(path + "\\" + filename + index + ".json"))
                {
                    index++;
                }
                filename = filename + index;
            }
            StreamWriter file = File.CreateText(path + "\\" + filename + ".json");
            file.Close();
            return filename;
        }

        private void WriteFileJson(string filename, string path, JsonObject inf)
        {
            using (StreamWriter file = new StreamWriter(path + "\\" + filename + ".json"))
            {
         
                file.WriteLine(inf);
                file.Close();

            }    
        }

        public Project() { }

        public void ToJson()
        {

          /*  KeyValuePair<String, JsonValue> obj = new KeyValuePair<String, JsonValue>("fkkjds", new JsonObject
            {
                ["Id"] = 123,
                ["Name"] = "const"
            });


            KeyValuePair<String, JsonValue> obj = new KeyValuePair<String, JsonValue>("fkkjds", new JsonValue
            {
                ["Id"] = 123,
                ["Name"] = "const"
            });
*/


            JsonObject projectModuls = new JsonObject();
            JsonArray projectFiles = new JsonArray();
            //Объект проекта, он один
            JsonObject ProjectJson = new JsonObject

            {
                //Информация о проекте
                ["Name"] = Name,
                ["CreationDate"] = CreationDate,
                ["ChangeDate"] = ChangeDate,

                //Список файлов проекта
                ["ListAllFiles"] = new JsonArray(),

                //Список моделей проекта
                ["ModelsInProject"] = new JsonObject()

            };



            var options = new JsonSerializerOptions { WriteIndented = true };
            MessageBox.Show(ProjectJson.ToJsonString(options));
        }
        public void ToJson11()
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
                ["ModelsInProject"] = new JsonObject { }
                //Пример модели
                /*            ["Model1"] = new JsonObject
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
                            }*/


            };

            JsonObject ray = obj!["ModelsInProject"]!.AsObject();
            string nameId = "3456";

            JsonObject array = new JsonObject { ["Id"] = "hhh", ["Low"] = 20 };



            obj!["ModelsInProject"]![array!["Id"]!.GetValue<string>()] = array; //Добавить 


            obj!["ModelsInProject"]![nameId+"fddf"] = new JsonObject { ["Id"] = 888888889, ["Low"] = 20 };

            
            

            // obj["ModelsInProject"] = ray;

            //Добавить
            //    obj!["ModelsInProject"]![nameId] = new JsonObject { ["Id"] = 1289, ["Low"] = 20 };

            //Удалить
            // obj!["ModelsInProject"].Remove("Model2");
            //  (JsonObject)obj["ModelsInProject"].Remove("Model2");

            // CreateDirectory(path + "\\" + "test");

            // string name = CreateFile("test", path);

            //  WriteFileJson(name, path, obj);
            /*
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
