using ForresterModeller.Windows.Views;
using Microsoft.Win32;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using ForresterModeller.src.Interfaces;

namespace ForresterModeller.src.Windows.ViewModels
{
    public class StartWindowViewModel : ReactiveObject, IJSONSerializable
    {
        /// <summary>
        /// Имя файла, хранящего информацию о последних проектах
        /// </summary>
        string FileName = "LastProjects";
        string FilePath = Directory.GetCurrentDirectory() + "\\";
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
                    MainWindow mainWindow = new MainWindow(openFileDialog.FileName);
                    this.DialogResult = true;
                    mainWindow.ShowDialog();
                    //ИЗМЕНИТЬ СОДЕРЖИМОЕ ОКНА ЕЩЕ
                }
            });

            CreateProjectCommand = ReactiveCommand.Create<Unit>(u =>
            {
                CreateProject proj = new CreateProject();
                var window = proj as Window;
                var dialogResult = window.ShowDialog();
                if (dialogResult == true) {
                    MainWindow mainWindow = new MainWindow(proj.ViewModel.FileName);
                    this.DialogResult = true;
                    mainWindow.ShowDialog();
                    //ИЗМЕНИТЬ СОДЕРЖИМОЕ ОКНА ЕЩЕ
                }
            });

        }

        public JsonObject ToJSON()
        {
            throw new NotImplementedException();
        }

        public void FromJSON(JsonObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
