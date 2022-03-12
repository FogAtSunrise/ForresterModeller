using ForresterModeller.Entities;
using System.Collections.Generic;
using System.Windows.Controls;
using WPFtest1.src.Entities;

namespace ForresterModeller.src.Tools
{
    /// <summary>
    /// Логика взаимодействия для ToolsPage.xaml
    /// </summary>
    public partial class ToolsPage : Page
    {
        private int countColumn;
        public ToolsPage()
        {
            InitializeComponent();
            countColumn = 0;
        }

        /// <summary>
        /// вывод элементов графика из одной диаграммы, по принадлежности к классу или
        /// абстрактному классу определяет в соответствующую группу
        /// </summary>
        /// <param name="elements"></param>
        // public void ChangeListInPlotterTools(List<IDiagramEntity> elements)
        public void ChangeListInPlotterTools(List<IDiagramEntity> elements, string name)
        {
            //string name = "diagram1"+countColumn++;
            
 
            TreeViewItem treeHead = new TreeViewItem() { Header = name };
                      TreeViewItem tree1 = new TreeViewItem() { Header = "Уровни" };
                       TreeViewItem tree2 = new TreeViewItem() { Header = "Уравнения" };

                       foreach (var elem in elements)
                           if (elem is DiagramLevel)
                               tree1.Items.Add(new TreeViewItem() { Header = elem.Name});
                           else if (elem is DiagramFunction)
                               tree2.Items.Add(new TreeViewItem() { Header = elem.Name });

                       treeHead.Items.Add(tree1);
                       treeHead.Items.Add(tree2);
               
 
            
            TreeViewEvent.Items.Add(treeHead);

         
        }

     

    }
}
