using System;
using ForresterModeller.src.Interfaces;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Windows.ViewModels;
using ForresterModeller.src.Windows.Views;
using iTextSharp.text.pdf;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Windows.Controls;
using iTextSharp.text;

namespace ForresterModeller.src.ProjectManager.WorkArea
{
    public class MatViewManager : WorkAreaManager
    {
        public ObservableCollection<MathViewModel> Models { get; set; } = new();
        public override ContentControl Content => GenerateActualView();
        public DiagramManager Diagram { get; set; }

        public ReactiveCommand<Unit, Unit> UpdateMath { get; }
        public ReactiveCommand<Unit, Unit> pdfConvert { get; }

        public MatViewManager(DiagramManager dmanager)
        {
            Diagram = dmanager;
            dmanager.MathVM = this;
            Name = "Матпредставление для " + Diagram.Name;
            dmanager.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(DiagramManager.Name))
                    Name = "Матпредставление для " + Diagram.Name;
            };

            var mod = dmanager.АllNodes;
            if (mod != null)
                foreach (var mod1 in mod)
                {
                    if (!(mod1 is CrossNodeModel))
                        Models.Add(new MathViewModel(mod1));
                }

            UpdateMath = ReactiveCommand.Create<Unit>(u => updatemod());
            pdfConvert = ReactiveCommand.Create<Unit>(u => getPDF());

        }

        public void getPDF()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string path = dlg.FileName;

                 String FONT = "arial.ttf";

                var document = new iTextSharp.text.Document();
                using (var writer = PdfWriter.GetInstance(document, new FileStream(path+".pdf", FileMode.Create)))
                {
                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(
                        iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font CompanyFont = new iTextSharp.text.Font(
                        iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font HeaderGrowerFont = new iTextSharp.text.Font(
                        iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                    document.Open();


                    BaseFont basefont = BaseFont.CreateFont("arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);


                   // BaseFont baseFont = BaseFont.CreateFont("arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                    //Header ---------------------------------------------------------------

                    PdfPTable tblHeader = new PdfPTable(1);
                    tblHeader.WidthPercentage = 100;


                    PdfPCell DiagramName =
                        new PdfPCell(new Phrase(Name, HeaderGrowerFont));
                    DiagramName.BorderWidth = 0;
                    DiagramName.HorizontalAlignment = Element.ALIGN_CENTER;
                    DiagramName.Colspan = 1;
                    DiagramName.FixedHeight = 40f;

                    tblHeader.AddCell(DiagramName);
                    document.Add(new Paragraph(" "));

                    foreach (var item in Models)
                    {

                        PdfPCell str = new PdfPCell(new Phrase(item.NodeForMod.Name, CompanyFont));
                        str.HorizontalAlignment = Element.ALIGN_LEFT;
                        str.BorderWidth = 1;
                        str.Colspan = 1;
                        str.FixedHeight = 30f;

                        tblHeader.AddCell(str);
                        document.Add(new Paragraph(" "));

                        str = new PdfPCell(new Phrase("Описание:", CompanyFont));

                     

                        str.BorderWidth = 0;
                        str.HorizontalAlignment = Element.ALIGN_LEFT;
                        str.Colspan = 1;
                        str.FixedHeight = 20f;

                        tblHeader.AddCell(str);
                        document.Add(new Paragraph(" "));

                        str = new PdfPCell(new Phrase(item.NodeForMod.Description, CompanyFont));
                        str.HorizontalAlignment = Element.ALIGN_LEFT;
                        str.BorderWidth = 0;
                        str.Colspan = 1;
                        str.FixedHeight = 20f;

                        tblHeader.AddCell(str);
                        document.Add(new Paragraph(" "));



                        foreach (var item2 in item.Data)
                        {
                            str = new PdfPCell(new Phrase(item2.Left+item2.sim+item2.Right, _standardFont));
                            str.HorizontalAlignment = Element.ALIGN_LEFT;
                            str.BorderWidth = 0;
                            str.Colspan = 1;
                            str.FixedHeight = 20f;

                            tblHeader.AddCell(str);
                            document.Add(new Paragraph(" "));

                        }



                    }

                    document.Add(tblHeader);

                   document.Close();
                    writer.Close();

                }
            }
        }

        public void updatemod()
        {
            Name = "Матпредставление для " + Diagram.Name;
            Diagram.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(DiagramManager.Name))
                    Name = "Матпредставление для " + Diagram.Name;
            };
            var mod = Diagram.АllNodes;
            Models.Clear();
            if (mod != null)
                foreach (var mod1 in mod)
                {
                    if (!(mod1 is CrossNodeModel))
                        Models.Add(new MathViewModel(mod1));
                }
        }
        public override string TypeName => "Математическое представление";


        public MathViewModel ActiveView
        {
            set => ActiveOwnerItem = value == null ? this : value.NodeForMod;
        }

        //ViewModel модели, свойства которой отображаются 
        //Биндится как SelectedItem во вью
        private IPropertyOwner _activeItem;

        //Сама модель, владеющая проперти
        public override IPropertyOwner ActiveOwnerItem
        {
            get
            {
                if (_activeItem == null || _activeItem == null) return this;
                return _activeItem;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _activeItem, value);
                OnPropertySelected(_activeItem);
            }
        }
        public MathView GenerateActualView()
        {
            MathView m = new MathView();
            m.DataContext = this;
            return m;
        }
        /*  public MathView GenerateActualView()
          {
              MathView m = new MathView();
              m.DataContext = this;
              return m;
          }
        */
        //свойства самого матпредставления
        public override ObservableCollection<PropertyViewModel> GetProperties()
        {
            var prop = base.GetProperties();
            return prop;
        }
    }
}
