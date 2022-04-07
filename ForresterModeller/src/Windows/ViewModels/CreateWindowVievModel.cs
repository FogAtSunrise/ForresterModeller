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
    public class CreateWindowVievModel : ReactiveObject
    {

      
        public ReactiveCommand<Unit, Unit> SetPath { get; }

        public ReactiveCommand<Unit, Unit> SaveProject { get; }
        public CreateWindowVievModel()
        {
           
            SetPath = ReactiveCommand.Create < Unit>(u => OpenGuide());
            SaveProject = ReactiveCommand.Create<Unit>(u => Save());
        }

  private string _nameFile = "";
        public string NameFile { get => _nameFile; set => this.RaiseAndSetIfChanged(ref _nameFile, value); }

        private string _pathToImage = "";
        public string PathToImage { get => _pathToImage; set => this.RaiseAndSetIfChanged(ref _pathToImage, value); }

        public string FileName = "";

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { this.RaiseAndSetIfChanged(ref _dialogResult, value); }
        }
        private void OpenGuide()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathToImage = fbd.SelectedPath;
            }

        }

        private void Save()
        {
            /* if (name_project.Text == "" || name_project.Text == null)
                 name_project.BorderBrush = Brushes.Red;
             else
             if (path_to_project.Text == "" || path_to_project.Text == null)
                 path_to_project.BorderBrush = Brushes.Red;
             else
             {
                 path_to_project.BorderBrush = Brushes.Black;
                 name_project.BorderBrush = Brushes.Black;

             */
            Project project = new Project(_nameFile, _pathToImage == "" ? "": (_pathToImage + "\\" + _nameFile));
                project.SaveNewProject();
                FileName = project.getPath() + "\\" + project.getName() + ".json";
            this.DialogResult = true;

        }
    }
}
