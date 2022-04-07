using ForresterModeller.Windows.Views;
using Microsoft.Win32;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ForresterModeller.src.Windows.ViewModels
{
    public class StartWindowViewModel : ReactiveObject
    {
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
        public ReactiveCommand<Unit, Unit> OpenProject { get; }

        /// <summary>
        /// Создать проект
        /// создает и инициализирует активный проект
        /// </summary>
        public ReactiveCommand<Unit, Unit> CreateProject { get; }

        public StartWindowViewModel()

        {
            OpenProject =  ReactiveCommand.Create<Unit>(u => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Файлы json|*.json";
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() ==true)
                {
                    System.Windows.MessageBox.Show("Открылся"+ openFileDialog.FileName);////////////////////////////////
                    MainWindow dialog = new MainWindow(openFileDialog.FileName);
                this.DialogResult = true;
                dialog.ShowDialog();
                   

                    //ИЗМЕНИТЬ СОДЕРЖИМОЕ ОКНА ЕЩЕ
                }
            });
        
                   CreateProject = ReactiveCommand.Create<Unit>(u => {

             
    CreateProject proj = new CreateProject();
    var window = proj as Window;
    var dialogResult = window.ShowDialog();

                if (dialogResult == true)

                {
                           MainWindow dialog = new MainWindow(proj.ViewModel.FileName);
                           this.DialogResult = true;
                           dialog.ShowDialog();
                    //ИЗМЕНИТЬ СОДЕРЖИМОЕ ОКНА ЕЩЕ
                }
            });
 
        }
     
    }
}
