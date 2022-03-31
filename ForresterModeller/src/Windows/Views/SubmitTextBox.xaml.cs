using System.Windows.Controls;
using System.Windows.Input;

namespace ForresterModeller.Windows.Views
{
    /// <summary>
    /// Interaction logic for SubmitTextBox.xaml
    /// </summary>
    public partial class SubmitTextBox : TextBox
    {
        public SubmitTextBox()
        {
            InitializeComponent();
        }

        private void SubmitTextBoxOnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox? s = e.Source as TextBox;
                s?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                e.Handled = true;
            }
        }
    }
}
