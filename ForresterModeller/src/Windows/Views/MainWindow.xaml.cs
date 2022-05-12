using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ForresterModeller.src.Interfaces;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.ProjectManager;
using ForresterModeller.src.ProjectManager.miniParser;
using ForresterModeller.src.ProjectManager.WorkArea;
using ForresterModeller.src.Windows.ViewModels;
using ReactiveUI;
using WpfMath.Controls;

namespace ForresterModeller.src.Windows.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(StartWindowViewModel startWindowVM)
        {
            InitializeComponent();
            Project a = new Project { Name = "naaame of proj" };
            a.Diagrams.Add(new DiagramManager("1", a));
            a.Diagrams.Add(new DiagramManager("12", a));
            a.Diagrams.Add(new DiagramManager("123", a));
            DataContext = new MainWindowViewModel(a, startWindowVM);
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
            MinParser d = new MinParser();

            //  Result ho = Pars.CheckFormula(input_formul.Text.ToString());
            Result ho = d.CheckFormula(input_formul.Text.ToString());
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
                 MessageBox.Show("Давай по новой, Миша, *заблокировано РКН*! (если что, это тест на константу)"); */
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = ((TreeView)sender).SelectedItem;
            if (item is ForesterNodeModel node)
            {
                ((MainWindowViewModel)DataContext).SetSelectedNode(node);
                return;
            }

            if (item is WorkAreaManager diagram)
            {
                ((MainWindowViewModel)DataContext).OpenOrCreateTab(diagram);
                return;
            }

            if (item is IPropertyOwner propItem)
                ((MainWindowViewModel)DataContext).PropertiesVM.ActiveItem = propItem;

        }

        private void InfoGrid_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var mainVM = ((MainWindowViewModel)DataContext);
            if (e.Key == Key.Delete)
            {
                var grid = (Grid)sender;
                TreeView tree = null;
                foreach (var child in grid.Children)
                {
                    if (child is TreeView)
                    {
                        tree = (TreeView)child;
                    }
                }
                if (tree != null)
                {
                    var obj = (IPropertyOwner)tree.SelectedItem;
                    string checkMessage = "Удалить " + obj.TypeName + " \"" + obj.Name + "\"? Данное действие нельзя отменить.";
                    var result = MessageBox.Show(checkMessage, "Delete", MessageBoxButton.OKCancel);

                    if (result == MessageBoxResult.OK)
                    {
                        
                        ((MainWindowViewModel)DataContext).Remove(obj);
                        tree.ItemsSource = mainVM.ActiveProject?.Diagrams;
                        tree.Items.Refresh();
                        tree.UpdateLayout();
                    }
                
                }
            }
        }

    }
}
