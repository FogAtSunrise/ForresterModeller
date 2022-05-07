using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using ForresterModeller.src.ProjectManager;
using ForresterModeller.src.ProjectManager.miniParser;
using ForresterModeller.src.ProjectManager.WorkArea;
using ForresterModeller.src.Windows.ViewModels;
using WpfMath.Controls;

namespace ForresterModeller.Windows.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow(StartWindowViewModel startWindowVM)
        {
            InitializeComponent();
            Project a = new Project{Name = "naaame of proj"};
            a.Diagrams.Add(new DiagramManager("1", a));
            a.Diagrams.Add(new DiagramManager("12", a));
            a.Diagrams.Add(new DiagramManager("123", a));
            DataContext = new MainWindowViewModel(a, startWindowVM);
            //OpenPageInFrame(ToolsFrame, new DiagramTools());
            //тест вывода формулы
            PrintFormule(@"\frac{\pi}{a^{2n+1}} = 0");
            PrintFormule(@"x_{t_i}=x_{t_{i+1}}*12");
        }

        public MainWindow(string path, StartWindowViewModel startWindowVM)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(Loader.InitProjectByPath(path), startWindowVM); ;
        }

        private void PrintFormule(string form)
        {
            FormulaControl forml = new FormulaControl();
            forml.Formula = form;
            formuls.Children.Add(forml);
        }
        private void Button_Click_Add_Formule(object sender, RoutedEventArgs e)
        {
            MinParser b = new MinParser();
           
            Result ho = b.check(input_formul.Text.ToString());
             if (ho.result)
             {
                 PrintFormule(input_formul.Text.ToString());
                 input_formul.Text = "";
             }
             else
                 MessageBox.Show(ho.str);
           /* 
             bool r = b.isCorrect("Значение", input_formul.Text.ToString());
            if (r)
            {
                PrintFormule(input_formul.Text.ToString());
                input_formul.Text = "";
            }
            else
                MessageBox.Show("Давай по новой, Миша, Все хуйня! (если что, это тест на константу)"); */
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = ((TreeView)sender).SelectedItem;
            if (item is WorkAreaManager)
              ((MainWindowViewModel)DataContext).OpenOrCreateTab((WorkAreaManager)item);
        }
    }
}
