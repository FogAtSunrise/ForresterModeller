using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ForresterModeller.src.Pages.Properties;
using ForresterModeller.src.Pages.Tools;
using ForresterModeller.src.Tools;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.ProjectManager;
using ForresterModeller.src.ProjectManager.WorkArea;
using WpfMath.Controls;
using System.Text.Json;
using System.Text.Json.Nodes;
using ForesterNodeCore ;

namespace ForresterModeller
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApplicationManager manager = new();
        private ActionTabViewModal OpenedPages;

        public MainWindow()
        {
            InitializeComponent();
            OpenedPages = new ActionTabViewModal(PagesTabControl);
        
           // OpenedPages.Populate();
           ConstantNodeViewModel ctx = new ConstantNodeViewModel("DIR", "sur", 12);
           OpenProperty(ctx);


            ChangeListInFileManager(new List<string> { "file1", "file2", "file3" }, "project1");
            //тест вывода формулы
            PrintFormule(@"\frac{\pi}{a^{2n+1}} = 0");
            PrintFormule(@"x_{t_i}=x_{t_{i+1}}*12");
        }

        private void OpenProperty(IPropertyChangable objChangable)
        {
            OpenPageInFrame(PropertyFrame, new PropertyTemplate(objChangable));
        }

        private void Test1(object sender, RoutedEventArgs e)
        {
            // openNewPage("file_name");
            // LeftBelowFrame.NavigationService.Navigate(new LeftBelow.GraphElements());
            OpenPageInFrame(ToolsFrame, new GraphElements());

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
                MessageBox.Show(el.ToJSON().ToJsonString(options));
               
                
                JsonObject test = el.ToJSON();
                test["Name"] = "Another name";
            
                el.FromJSON(test);
                options = new JsonSerializerOptions { WriteIndented = true };
                MessageBox.Show(el.ToJSON().ToJsonString(options));
               
            }
        }
        //фрейм plottertools
        private void Test2(object sender, RoutedEventArgs e)
        {

            PlotterTools t = new PlotterTools();
            List<ForesterNodeModel> test = new List<ForesterNodeModel>();
            for (int i = 0; i < 6; i++)
                test.Add(new LevelNodeModel());

            for (int i = 0; i < 6; i++)
                t.ChangeListInPlotterTools(test, "name" + i);

            OpenPageInFrame(ToolsFrame, t);
        }
        /// <summary>
        /// ОТКРЫТЬ УКАЗАННУЮ СТРАНИЦУ, В УКАЗАННОМ ФРЕЙМЕ
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="page"></param>
        private void OpenPageInFrame(Frame frame, Page page)
        {
            frame.NavigationService.Navigate(page);
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

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            OpenedPages.Tabs.RemoveAt(PagesTabControl.SelectedIndex);
            
           
        }

        /// <summary>
        /// Открыть вкладку в табах
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        private void OpenNewPage(WorkAreaManager workareafile)
        {
            //OpenedPages.add(name, manager.CreateContentControl(type));
            OpenedPages.add(workareafile); 
            OpenedPages.tc.SelectedIndex = OpenedPages.Tabs.Count - 1;
            var active = OpenedPages.Tabs[PagesTabControl.SelectedIndex];
            OpenProperty(active.wamanager);

        }

        private void TestPlot(object sender, RoutedEventArgs e)
        {
           // OpenNewPage(new PlotManager());
        }
        private void TestGraf(object sender, RoutedEventArgs e)
        {
            OpenNewPage(new DiagramManager{Name = "D12"});
        }

        private void PrintFormule(string form)
        {
            FormulaControl forml = new FormulaControl();
            forml.Formula = form;
            formuls.Children.Add(forml);
        }
        private void Button_Click_Add_Formule(object sender, RoutedEventArgs e)
        {
            PrintFormule(input_formul.Text.ToString());
            input_formul.Text = "";
        }

        private void MenuGetGraphics_OnClick(object sender, RoutedEventArgs e)
        {
            float t = 1, dt = 0.1f;
            var c = ForesterNodeCore.Program.GetCurve(
                "c a 1 | c b 2 | l dc b a 0 | f nt dc/2 | d boo loo 1 b nt 0",
                new List<NodeIdentificator> {
                    new NodeIdentificator("dc"),
                    new NodeIdentificator("nt"),
                    new NodeIdentificator("boo"),
                    new NodeIdentificator("loo"),
                },
                t,
                dt
            );
            PlotManager plotmodel = new (c, t, dt);
            plotmodel.XLabel = "Время (недели)FFF";
            plotmodel.YLabel = "Объем товара (единицы) ";
            plotmodel.Name = "График123";
            OpenNewPage(plotmodel);
        }
    }
}
