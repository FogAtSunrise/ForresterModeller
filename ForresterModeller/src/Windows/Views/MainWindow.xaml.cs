using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using ForresterModeller.Pages.Tools;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Pages.Tools;
using ForresterModeller.src.ProjectManager;
using ForresterModeller.src.ProjectManager.MiniParser;
using ForresterModeller.Windows.ViewModels;
using WpfMath.Controls;

namespace ForresterModeller.Windows.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApplicationManager manager = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(new Project());
            //OpenPageInFrame(ToolsFrame, new DiagramTools());
            ChangeListInFileManager(new List<string> { "file1", "file2", "file3" }, "project1");
            //тест вывода формулы
            PrintFormule(@"\frac{\pi}{a^{2n+1}} = 0");
            PrintFormule(@"x_{t_i}=x_{t_{i+1}}*12");
        }

        public MainWindow(string path)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(Loader.InitProjectByPath(path)); ;
        }

        private void Test1(object sender, RoutedEventArgs e)
        {
            // openNewPage("file_name");
            // LeftBelowFrame.NavigationService.Navigate(new LeftBelow.GraphElements());

            List<IForesterModel> elem = new List<IForesterModel>() {
                new LevelNodeModel("lev1", "levelishe1", "in", "out"),
                new ConstantNodeViewModel("const1", "constanta", 6.8f),
                new ChouseNodeModel("chous", "comment", "34x^2+11*6x=34"),
                new FunkNodeModel("chous", "comment", "34x^2+11*6x=34"),
                new DelayNodeModel(),
            };

            foreach (var el in elem)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                //MessageBox.Show(el.ToJSON().ToJsonString(options));


                JsonObject test = el.ToJSON();
                test["Name"] = "Another name";

                el.FromJSON(test);
                options = new JsonSerializerOptions { WriteIndented = true };
                //MessageBox.Show(el.ToJSON().ToJsonString(options));

            }
        }


        /// <summary>
        /// вывод файлов проекта
        /// </summary>
        /// elements - список имен файлов
        /// name - имяпроекта

        public void ChangeListInFileManager(List<string> elements, string name)
        {
            TreeFiles.Items.Clear();

            TreeViewItem treeHead = new TreeViewItem() { Header = name };
            foreach (var elem in elements)
            {
                TreeViewItem treeItem = new TreeViewItem();
                treeItem.Header = elem;
                // treeItem.MouseLeftButtonUp += OpenFile_MouseLeftButtonUp;
                treeItem.MouseDoubleClick += OpenFile_MouseLeftButtonUp;
                treeHead.Items.Add(treeItem);

            }
            TreeFiles.Items.Add(treeHead);
        }


        /// <summary>
        /// Ловит, какой узел был нажат в дереве, тобишь определяет,
        /// какой файл проекта пользователь хочет открыть через файловый менеджер 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OpenFile_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            //  MessageBox.Show("Должен открыться " + item.Header);


        }

        private void PrintFormule(string form)
        {
            FormulaControl forml = new FormulaControl();
            forml.Formula = form;
            formuls.Children.Add(forml);
        }
        private void Button_Click_Add_Formule(object sender, RoutedEventArgs e)
        {
            MinParser p = new MinParser();
           // p.isInputCorrect(input_formul.Text);
            PrintFormule(input_formul.Text.ToString());
            input_formul.Text = "";
        }


    }
}
