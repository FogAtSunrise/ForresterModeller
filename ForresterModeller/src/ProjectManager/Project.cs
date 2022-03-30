
using ForresterModeller.src.Nodes.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        string DefaultPath = Directory.GetCurrentDirectory() + "\\";

        List<IForesterModel> allProjectModels = new List<IForesterModel>();
        List<string> listAllFiles = new List<string>();


        string Name;
        public string getName() { return Name; }


        DateTime CreationDate;
        public DateTime getCreationDate() { return CreationDate; }
        DateTime ChangeDate;
        public DateTime getChangeDate() { return ChangeDate; }

        /// <summary>
        /// хранит директорию, в которой хранится json проекта!!!
        /// </summary>
        string PathToProject;


        /// <summary>
        /// Добавить в проект новую модель
        /// </summary>
        /// <param name="mod"></param>
        public void addModel(IForesterModel model)
        {
            Random rnd = new Random();
            model.Id = model.Id + rnd.Next();

            allProjectModels.Add(model);
        }
        /// <summary>
        /// добавить имя файла в список файлов проекта
        /// </summary>
        /// <param name="name"></param>
        public void addFiles(string name)
        {

            listAllFiles.Add(name);
        }
        /// <summary>
        /// конструктор, принимает путь включающий имя json проекта, для существующего файла
        /// </summary>
        /// <param name="path"></param>
        public Project(string path):this( Path.GetFileNameWithoutExtension(path), Path.GetDirectoryName(path))
        {
            openOldProject();
        }


        public Project(string name, string pathTofile)
        {
            Name = (name == null || name == "") ? DefaultName : name;
            PathToProject = (pathTofile == null || pathTofile == "") ? DefaultPath + Name : pathTofile;
            CreationDate = DateTime.Now;
            ChangeDate = DateTime.Now;
        }

        public Project()
        {
            Name = DefaultName;
            PathToProject = DefaultPath + Name;
            CreationDate = DateTime.Now;
            ChangeDate = DateTime.Now;

        }

  
        public void openOldProject()
        {
            using (StreamReader r = new StreamReader(PathToProject + "\\" + Name + ".json"))
            {
               string json = r.ReadToEnd();
               
               var jobj = JsonObject.Parse(json); 
                r.Close();
                FromJson(jobj.AsObject());
                var options = new JsonSerializerOptions { WriteIndented = true };////////////////////
                MessageBox.Show(jobj.ToJsonString(options));
            }
        }


        public void SaveOldProject()
        {
            if (Directory.Exists(PathToProject))
            {
                WriteFileJson(Name, PathToProject, ToJson());
            }
            else SaveNewProject();
        }

        public void SaveNewProject()
        {

            string ind = CreateDirectory(PathToProject);
            Name += ind;
            PathToProject += ind;
            CreateFile(Name, PathToProject);
            JsonObject jsonVerst = ToJson();
            WriteFileJson(Name, PathToProject, jsonVerst);

        }

        /// <summary>
        /// Создать новую директорию (не важно, для целого проекта или в самом проекте)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //пример вызова:   CreateDirectory(path + "\\" + "имя новой папки");
        private string CreateDirectory(string path)
        {
            int index = 0;
            if (Directory.Exists(path))
             {
                index = 1;

                while (Directory.Exists(path + index))
                 {
                     index++;
                 }
                 path = path + index;
                
            }
             Directory.CreateDirectory(path);
             return (index==0)?"": index.ToString();
        }


            /// <summary>
            /// Создать файл json для записи
            /// </summary>
            /// <param name="filename"></param>
            /// <param name="path"></param>
            /// <returns></returns>
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


        /// <summary>
        /// запись в файл json, причем он перезаписывает файл
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <param name="inf"></param>
        private void WriteFileJson(string filename, string path, JsonObject inf)
        {
            using (StreamWriter file = new StreamWriter(path + "\\" + filename + ".json", false))
            {
         
                file.Write(inf);
                file.Close();

            }    
        }

        /// <summary>
        /// переводит содержимое проекта в json, использовать при сохранении
        /// </summary>

       public  JsonObject  ToJson()
        {

            JsonArray projectFiles = new JsonArray();
            foreach (var file in listAllFiles)
            {
                projectFiles.Add(file);
            }

            JsonArray projectModuls = new JsonArray();


            foreach (var model in allProjectModels)
            {
                projectModuls.Add(model.ToJSON());
              

            }

            //Объект проекта, он один
            JsonObject ProjectJson = new JsonObject

            {
                //Информация о проекте
                ["Name"] = Name,
                ["CreationDate"] = CreationDate,
                ["ChangeDate"] = DateTime.Now,

                //Список файлов проекта
                ["ListAllFiles"] = projectFiles,

                //Список моделей проекта
                ["ModelsInProject"] = projectModuls

            };

            /*
                        JsonArray projectFiles = new JsonArray();
                        foreach (var file in listAllFiles)
                        {
                            projectFiles.Add(file);
                        }

                        JsonObject projectModuls = new JsonObject();


                        foreach (var model in allProjectModels)
                        {
                            projectModuls![model.Id] = model.ToJSON();

                        }

                        //Объект проекта, он один
                        JsonObject ProjectJson = new JsonObject

                        {
                            //Информация о проекте
                            ["Name"] = Name,
                            ["CreationDate"] = CreationDate,
                            ["ChangeDate"] = DateTime.Now,

                            //Список файлов проекта
                            ["ListAllFiles"] = projectFiles,

                            //Список моделей проекта
                            ["ModelsInProject"] = projectModuls

                        };
            */
            return ProjectJson;
        }

        public void FromJson(JsonObject obj)
        {

            Name = obj!["Name"]!.GetValue<string>();
            CreationDate = obj!["CreationDate"]!.GetValue<DateTime>();


            //   JsonArray studentsArray = root["Students"]!.AsArray();

            string k = "";
            JsonArray projectFiles =obj["ListAllFiles"]!.AsArray();
            foreach (var file in projectFiles)
            {
                listAllFiles.Add(file.ToString());
            //    k += listAllFiles[listAllFiles.Count - 1]+"--";
            }

            //  MessageBox.Show(k);

            JsonArray projectModuls = obj["ModelsInProject"]!.AsArray();
            foreach (var model in projectModuls)
            {
           
                    k += model!["Name"]!.GetValue<string>() + "--";
            }

              MessageBox.Show(k);
            /* JsonObject projectModuls = new JsonObject();


             foreach (var model in allProjectModels)
             {
                 projectModuls![model.Id] = model.ToJSON();
             }

             //Объект проекта, он один
             JsonObject ProjectJson = new JsonObject

             {
                 //Информация о проекте
                 ["Name"] = Name,
                 ["CreationDate"] = CreationDate,
                 ["ChangeDate"] = DateTime.Now,

                 //Список файлов проекта
                 ["ListAllFiles"] = projectFiles,

                 //Список моделей проекта
                 ["ModelsInProject"] = projectModuls

             };

 */
        }
        public void ToJson11()
        {
           // string path = PathToProject;
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



            obj![array!["Id"]!.GetValue<string>()] = array; //Добавить 


         //   obj!["ModelsInProject"]![nameId+"fddf"] = new JsonObject { ["Id"] = 888888889, ["Low"] = 20 };

            
            

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

        /// <summary>
        /// создает модель нужного типа
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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
