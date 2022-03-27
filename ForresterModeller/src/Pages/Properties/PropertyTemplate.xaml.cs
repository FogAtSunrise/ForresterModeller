using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ForresterModeller.src.Nodes.Models;

namespace ForresterModeller.src.Pages.Properties
{
    /// <summary>
    /// Логика взаимодействия для PrpertyTemplate.xaml
    /// Запрашивает от полученной модели список свойств и
    /// передает их представлению.
    /// </summary>
    public partial class PropertyTemplate : Page
    {
        public ObservableCollection<Property> Properties { get; set; }

        private Binding nameBinding;
        private Binding typeNameBinding;

        public PropertyTemplate(ForesterNodeModel model)
        {
            nameBinding = new Binding("Name") { Source = model };
            typeNameBinding = new Binding("TypeName") { Source = model };
            DataContext = this;
            Properties = model.GetProperties();

            InitializeComponent();
            FullName.SetBinding(TextBlock.TextProperty, nameBinding);
            TypeName.SetBinding(TextBlock.TextProperty, typeNameBinding);
    
        }
    }
}
