
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

using MessageBox = System.Windows.MessageBox;

namespace ForresterModeller.src.ProjectManager
{
    public class Project
    {
        /// <summary>
        /// дефолтные значения имени и пути проекта
        /// </summary>
        string DefaultName = "New Project";
        string DefaultPath = Directory.GetCurrentDirectory() + "\\";

        /// <summary>
        /// список моделей
        /// </summary>
        List<ForesterNodeModel> allProjectModels = new List<ForesterNodeModel>();

        /// <summary>
        /// список файлов, описывающих диаграммы
        /// </summary>
        List<string> listAllFiles = new List<string>();

        //тест метод, удалите его обязательно
        public string getIdMod(int numb)
        {
            if (allProjectModels.Count >= numb)
                return allProjectModels[numb].Id;
            return "";
        }


        string Name;
        public string getName() { return Name; }

        DateTime CreationDate;
        public DateTime getCreationDate() { return CreationDate; }
        DateTime ChangeDate;
        public DateTime getChangeDate() { return ChangeDate; }

        /// <summary>
        /// хранит директорию, в которой хранится json проекта!!! т.е. не включает в себя имя json 
        /// </summary>
        string PathToProject;
        public string getPath() { return PathToProject; }

     
        /// <summary>
        /// конструкторы
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pathTofile"></param>
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
            PathToProject = DefaultPath+Name;
            CreationDate = DateTime.Now;
            ChangeDate = DateTime.Now;

        }


        /// <summary>
        /// Добавить в проект новую модель
        /// </summary>
        /// <param name="mod"></param>
        public void addModel(ForesterNodeModel model)
        {
            Random rnd = new Random();
            model.Id = model.Id + rnd.Next();

            allProjectModels.Add(model);
        }

        /// <summary>
        /// удаление модели по id
        /// </summary>
        /// <param name="id"></param>
        public void deleteModel(string id)
        {
            ForesterNodeModel find = allProjectModels.Find((item) => item.Id == id);
            if (find != null)
                allProjectModels.Remove(find);
           
        }
        /// <summary>
        /// добавить имя файла в список файлов проекта
        /// </summary>
        /// <param name="name"></param>
        public void addFiles(string name)
        {

            listAllFiles.Add(name);
            //...
        }


        public void deleteFile(string name)
        {
            listAllFiles.Add(name);
            //...
        }


 

        /// <summary>
        /// получить экземпляр модели по id
        /// если модель не найдена, вернет null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ForesterNodeModel getModelById(string id)
        {
            ForesterNodeModel find = allProjectModels.Find((item) =>  item.Id == id);
            return find;
        }

        /// <summary>
        /// сохранить изменения существующего проекта
        /// </summary>
        public void SaveOldProject()
        {
            if (Directory.Exists(PathToProject))
            {
                Loader.WriteFileJson(Name, PathToProject, ToJson());
            }
            else SaveNewProject();
        }

        /// <summary>
        /// сохранить новый проект
        /// </summary>
        public void SaveNewProject()
        {

            string ind = Loader.CreateDirectory(PathToProject);
            Name += ind;
            PathToProject += ind;
            Loader.CreateFile(Name, PathToProject);
            Loader.CreateDirectory(PathToProject + "\\diagrams");
            JsonObject jsonVerst = ToJson();
            Loader.WriteFileJson(Name, PathToProject, jsonVerst);

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

          
            return ProjectJson;
        }

        public void FromJson(JsonObject obj)
        {
            try
            {
                Name = obj!["Name"]!.GetValue<string>();
                CreationDate = obj!["CreationDate"]!.GetValue<DateTime>();

                //   JsonArray studentsArray = root["Students"]!.AsArray();

              //  string k = "";
                JsonArray projectFiles = obj["ListAllFiles"]!.AsArray();
                foreach (var file in projectFiles)
                {
                    listAllFiles.Add(file.ToString());
                    //    k += listAllFiles[listAllFiles.Count - 1]+"--";
                }

                //  MessageBox.Show(k);

                JsonArray projectModuls = obj["ModelsInProject"]!.AsArray();
                foreach (var model in projectModuls)
                {
                    ForesterNodeModel m = createModel(model!["Type"]!.GetValue<string>());
                    m.FromJSON(model.AsObject());
                    allProjectModels.Add(m);
                 //   k += m.TypeName + m.Id + "\n";
                }

                //   MessageBox.Show(k);

            }
            catch 
            {
                MessageBox.Show("Не верно выбран файл проекта");
            }
            
        }


        /// <summary>
        /// создает модель нужного типа
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ForesterNodeModel createModel(string type)
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
