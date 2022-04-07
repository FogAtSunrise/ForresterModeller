using ForresterModeller.src.ProjectManager;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForresterModeller.src.Windows.ViewModels
{
    public class CreateWindowViewModel : ReactiveObject
    {

      /// <summary>
      /// команда для выбора пути к проекту
      /// </summary>
        public ReactiveCommand<Unit, Unit> SetPath { get; }

        /// <summary>
        /// команда создания проекта по введенным данным
        /// </summary>
        public ReactiveCommand<Unit, Unit> SaveProject { get; }
        public CreateWindowViewModel()
        {
           
            SetPath = ReactiveCommand.Create < Unit>(u => OpenGuide());
            SaveProject = ReactiveCommand.Create<Unit>(u => Save());
        }
        /// <summary>
        /// результат работы формы, содержит полный путь к json созданного проекта
        /// </summary>
  private string _nameFile = "";

        /// <summary>
        /// содержит введенное имя
        /// </summary>
        public string NameFile { get => _nameFile; set => this.RaiseAndSetIfChanged(ref _nameFile, value); }

        private string _pathToImage = "";
        /// <summary>
        /// содержит указанный путь
        /// </summary>
        public string PathToImage { get => _pathToImage; set => this.RaiseAndSetIfChanged(ref _pathToImage, value); }

        public string FileName = "";

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
        /// метода выбора пути для соответствующей команды
        /// </summary>
        private void OpenGuide()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathToImage = fbd.SelectedPath;
            }

        }

       /// <summary>
       /// метод создания проекта
       /// НЕТ НЕОБХОДИМОСТИ ПРОВЕРЯТЬ ПОЛЯ ФОРМЫ, Т.К. С ПУСТЫМИ ПОЛЯМИ НАИМЕНОВАНИЕ И ПУТЬ БУДУТ ЗАДАНЫ ПО УМОЛЧАНИЮ!
       /// </summary>
        private void Save()
        {
            Project project = new Project(_nameFile, _pathToImage == "" ? "": (_pathToImage + "\\" + _nameFile));
                project.SaveNewProject();
                FileName = project.getPath() + "\\" + project.getName() + ".json";
            this.DialogResult = true;

        }
    }
}
