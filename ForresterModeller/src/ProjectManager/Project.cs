using ForresterModeller.src.Nodes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using System.Windows.Navigation;
using DynamicData;
using ForresterModeller.src.Interfaces;
using ForresterModeller.src.ProjectManager.miniParser;
using ForresterModeller.src.ProjectManager.WorkArea;
using NodeNetwork.Views;
using ReactiveUI;
using MessageBox = System.Windows.MessageBox;
using ForresterModeller.src.Windows.ViewModels;

namespace ForresterModeller.src.ProjectManager
{

    public class Project : ReactiveObject, IPropertyOwner
    {
        /// <summary>
        /// дефолтные значения имени и пути проекта
        /// </summary>
        static string DefaultName = "New Project";
        static string DefaultPath = Directory.GetCurrentDirectory() + "\\";
        private double _deltaTime = 0.1;
        private double _allTime = 10;

        public double DeltaTime
        {
            get => _deltaTime;
            set
            {
                _deltaTime = value;
                foreach (var diagram in Diagrams)
                {
                    diagram.DeltaTime = _deltaTime;
                }
            }
        }

        public double AllTime
        {
            get => _allTime;
            set
            {
                _allTime = value;
                foreach (var diagram in Diagrams)
                {
                    diagram.AllTime = _allTime;
                }
            }
        }
        /// <summary>
        /// список моделей
        /// </summary>
        List<ForesterNodeModel> allProjectModels = new List<ForesterNodeModel>();


        public StartWindowViewModel startVM { get; set; }
        private ObservableCollection<DiagramManager> _diagrams;
        public ReadOnlyObservableCollection<DiagramManager> Diagrams { get; }

        public void AddDiagram(DiagramManager diagram)
        {
            _diagrams.Add(diagram);
            diagram.AllTime = AllTime;
            diagram.DeltaTime = DeltaTime;
            diagram.PropertyChanged += DiagramOnPropertyChanged;
            var network = (NetworkView)diagram.Content;

        }

        private void DiagramOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is IPropertyOwner owner && e.PropertyName == nameof(DiagramManager.АllNodes))
            {
                OnPropertySelected(owner);
            }
        }

        public DiagramManager getDiagramFromFileByName(string d)
        {
            DiagramManager diagram = new DiagramManager(d, this);
            try
            {
                StreamReader r = new StreamReader(PathToProject + "\\diagrams\\" + d + ".json");
                string json = r.ReadToEnd();
                var jobj = JsonObject.Parse(json);
                r.Close();
                diagram.JsonToDiagram(jobj.AsObject());

            }
            catch (Exception e){
                MessageBox.Show("Ошибка файла диаграммы " + d);
            }

            return diagram;
        }

        public void writeDiagramToFile(DiagramManager diagram)
        {
            JsonObject json = diagram.DiagramToJson();

            if (!Directory.Exists(PathToProject + "\\diagrams"))
            {
                Loader.CreateDirectory(PathToProject + "\\diagrams");

            }
            Loader.WriteFileJson(diagram.Name, PathToProject + "\\diagrams", json);
            diagram.PathToFile = PathToProject + "\\diagrams";

        }

        public ObservableCollection<PropertyViewModel> GetProperties()
        {
            var properties = new ObservableCollection<PropertyViewModel>();
            properties.Add(new PropertyViewModel(Resource.name, Name, (String str) => { Name = str; }, Pars.CheckName));
            properties.Add(new PropertyViewModel("Период исследования", AllTime.ToString(), s => AllTime = Utils.GetDouble(s), Pars.CheckConst));
            properties.Add(new PropertyViewModel("DT", DeltaTime.ToString(), s => DeltaTime = Utils.GetDouble(s), Pars.CheckConst));
            return properties;
        }

        public string TypeName { get; }
        private string _name;

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
        public event IPropertyOwner.PropertySelectedEventHandler PropertySelectedEvent;
        public void OnPropertySelected(IPropertyOwner sender)
        {
            PropertySelectedEvent?.Invoke(sender);
        }

        public DateTime CreationDate { get; private set; }
        public DateTime ChangeDate { get; private set; }

        /// <summary>
        /// хранит директорию, в которой хранится json проекта!!! т.е. не включает в себя имя json 
        /// </summary>
        public string PathToProject { get; set; }

        public string FullName() => PathToProject + "\\" + Name + ".json";

        /// <summary>
        /// конструкторы
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pathTofile"></param>
        public Project(string name, string pathTofile, StartWindowViewModel startWindowVM)
        {
            _diagrams = new();
            Diagrams = new(_diagrams);
            startVM = startWindowVM;
            Name = (name == null || name == "") ? DefaultName : name;
            PathToProject = (pathTofile == null || pathTofile == "") ? DefaultPath + Name : pathTofile;
            CreationDate = DateTime.Now;
            ChangeDate = DateTime.Now;
        }

        public Project(StartWindowViewModel startWindowVM)
        {

            startVM = startWindowVM;
            Name = DefaultName;
            PathToProject = DefaultPath + Name;
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
        /// удаление узла
        /// </summary>
        public void Remove(ForesterNodeModel node)
        {
            foreach (var diagram in Diagrams)
            {
                foreach (var nod in diagram.АllNodes)
                {
                    if (nod == node)
                    {
                      diagram.Content.ViewModel.Nodes.Remove(node);
                        diagram.UpdateNodes();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// удаление диаграммы
        /// </summary>
        public void Remove(DiagramManager deletedDiagram)
        {
            foreach (var diagram in Diagrams)
            {
                if (diagram == deletedDiagram)
                {
                    File.Delete(diagram.FullName);
                    _diagrams.Remove(diagram);
                    return;
                }
            }
        }
        /// <summary>
        /// удаление проекта
        /// </summary>
        public void RemoveFromFileSystem()
        {
            for (int i = Diagrams.Count - 1; i >= 0; i--)
            {
                Remove(Diagrams[i]);
            }
            File.Delete(this.FullName());
        }

        /// <summary>
        /// получить экземпляр модели по id
        /// если модель не найдена, вернет null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ForesterNodeModel getModelById(string id)
        {
            foreach (var diag in Diagrams)
            {
                diag.UpdateNodes();
                var node = diag.АllNodes.FirstOrDefault(x => x.Id == id);
                if (node != null)
                    return node;
            }
            return null;
        }
        /// <summary>
        /// получить экземпляр модели по обозначению
        /// если модель не найдена, вернет null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ForesterNodeModel getModelByName(string name)
        {
            foreach (var diag in Diagrams)
            {
                diag.UpdateNodes();
                var node = diag.АllNodes.FirstOrDefault(x => x.Name == name);
                if (node != null)
                    return node;
            }
            return null;
        }
        /// <summary>
        /// сохранить изменения существующего проекта
        /// </summary>
        public void SaveOldProject(StartWindowViewModel startVM)
        {
            ChangeDate = DateTime.Now;
            if (Directory.Exists(PathToProject))
            {
                Loader.WriteFileJson(Name, PathToProject, ToJson());
            }
            else SaveNewProject();
            startVM.AddProject(PathToProject + "\\" + Name + ".json");
            System.Windows.MessageBox.Show("Проект \"" + Name + "\" сохранён.");
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
            startVM.AddProject(FullName());
        }



        /// <summary>
        /// переводит содержимое проекта в json, использовать при сохранении
        /// </summary>

        public JsonObject ToJson()
        {

            JsonArray projectFiles = new JsonArray();
            foreach (var file in Diagrams)
            {
                projectFiles.Add(file.Name);
                writeDiagramToFile(file);
            }

            JsonArray projectModuls = new JsonArray();

            //Объект проекта, он один
            JsonObject ProjectJson = new JsonObject
            {
                //Информация о проекте
                ["Name"] = Name,
                ["Time"] = AllTime,
                ["Delta"] = DeltaTime,
                ["CreationDate"] = CreationDate,
                ["ChangeDate"] = DateTime.Now,
                //Список файлов проекта
                ["ListAllFiles"] = projectFiles
            };
            return ProjectJson;
        }

        public void FromJson(JsonObject obj)
        {
            try
            {
                Name = obj!["Name"]!.GetValue<string>();
                AllTime = obj!["Time"]!.GetValue<float>();
                DeltaTime = obj!["Delta"]!.GetValue<float>();
                CreationDate = obj!["CreationDate"]!.GetValue<DateTime>();
                ChangeDate = obj!["ChangeDate"]!.GetValue<DateTime>();

                JsonArray projectFiles = obj["ListAllFiles"]!.AsArray();
                foreach (var file in projectFiles)
                {
                    DiagramManager d = getDiagramFromFileByName(file.ToString());
                    AddDiagram(d);
                }
                ConnectLinkNodes();
            }
            catch
            {
                MessageBox.Show("Не верно выбран файл проекта");
            }
        }

        private void ConnectLinkNodes()
        {
            foreach (var diagram in Diagrams)
            {
                foreach (var node in diagram.Content.ViewModel.Nodes.Items)
                {
                    if (node is LinkNodeModel)
                    {
                        ((LinkNodeModel)node).AutoConectionLinks();
                    }
                }
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
