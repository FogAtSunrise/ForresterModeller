using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
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
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

namespace ForresterModeller.src.Windows
{
    /// <summary>
    /// Логика взаимодействия для HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {

        public HelpWindow()
        {
            InitializeComponent();
        }

        public HelpWindow(FixedDocumentSequence fds)
        {
            InitializeComponent();
            this.docViewer.Document = fds;
        }

        public HelpWindow(Stream stream)
        {
            var package = Package.Open(stream);
            string uri = "memorystream://myXps.xps";
            Uri packageUri = new Uri(uri);
            PackageStore.AddPackage(packageUri, package);
            XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.Maximum, uri);
            FixedDocumentSequence fds = xpsDocument.GetFixedDocumentSequence();
            this.Closed += (s,e) => stream.Close();
        }


    }
}
