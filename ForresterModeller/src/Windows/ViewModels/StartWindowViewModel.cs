using ForresterModeller.Windows.Views;
using Microsoft.Win32;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Windows;
using ForresterModeller.src.ProjectManager;

namespace ForresterModeller.src.Windows.ViewModels
{
    public class StartWindowViewModel : ReactiveObject
    {
        private List<Project> _lastProjects;
        public List<Project> LastProjects
        {
            get => _lastProjects;
            private set => this.RaiseAndSetIfChanged(ref _lastProjects, value);
        }
        /// <summary>
        /// Имя файла, хранящего информацию о последних проектах
        /// </summary>
        static string FileName = "LastProjects.txt";
        private static string FullPath
        {
            get => Directory.GetCurrentDirectory() + "\\" + FileName;
        }
        /// <summary>
        /// результат работы формы, если была нажата кнопка "сохранить", принимает значение true
        /// </summary>
        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { this.RaiseAndSetIfChanged(ref _dialogResult, value); }
        }

        /// <summary>
        /// Открыть существующий проект
        /// Открывает диаологовое окно, по выбранному json инициализирует активный проект необходимыми данными
        /// </summary>
        public ReactiveCommand<Unit, Unit> OpenProjectCommand { get; }

        /// <summary>
        /// Создать проект
        /// создает и инициализирует активный проект
        /// </summary>
        public ReactiveCommand<Unit, Unit> CreateProjectCommand { get; }

        public StartWindowViewModel()
        {
            OpenProjectCommand = ReactiveCommand.Create<Unit>(u =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Файлы json|*.json";
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == true)
                {
                    OpenProject(openFileDialog.FileName);
                }
            });

            CreateProjectCommand = ReactiveCommand.Create<Unit>(u =>
            {
                CreateProject proj = new CreateProject();
                var window = proj as Window;
                var dialogResult = window.ShowDialog();
                if (dialogResult == true)
                {
                    OpenProject(proj.ViewModel.FileName);
                }
            });

            LastProjects = GetActualProjectList();

        }

        public void OpenProject(string pathToProj)
        {
            if (File.Exists(pathToProj))
            {
                MainWindow mainWindow = new MainWindow(pathToProj, this);
                AddProject(pathToProj);
                this.DialogResult = true;
                mainWindow.ShowDialog();
                //ИЗМЕНИТЬ СОДЕРЖИМОЕ ОКНА ЕЩЕ
            }
        }
        private static List<Project> GetActualProjectList()
        {
            var uniquePaths = new HashSet<string>();
            var projects = new List<Project>();
            if (!File.Exists(FullPath))
            {
                File.Create(FullPath);
                return projects;
            }
            using (var sr = new StreamReader(FullPath))
            {
                while (!sr.EndOfStream)
                {
                    var path = sr.ReadLine();
                    if (File.Exists(path))
                        uniquePaths.Add(path);
                }
                sr.Close();
            }

            using (var sw = new StreamWriter(FullPath, false))
            {
                foreach (var projPath in uniquePaths)
                {
                    projects.Add(Loader.InitProjectByPath(projPath));
                    sw.WriteLine(projPath);
                }
                sw.Close();
            }
            projects.Sort((a, b) => a.ChangeDate.CompareTo(b.ChangeDate));
            return projects;
        }

        public void AddProject(string PathToProjectJson)
        {
            using (StreamWriter file = new StreamWriter(FullPath, true))
            {
                file.WriteLine(PathToProjectJson);
                file.Close();
                LastProjects = GetActualProjectList();
            }
        }
    }
}
