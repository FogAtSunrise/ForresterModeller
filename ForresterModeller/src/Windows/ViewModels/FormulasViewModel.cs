using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ForresterModeller.src.Interfaces;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.ProjectManager.WorkArea;
using ForresterModeller.src.Windows.Views;
using ReactiveUI;

namespace ForresterModeller.src.Windows.ViewModels
{
    public class FormulasViewModel : ReactiveObject
    {
        public string Name;
        public ObservableCollection<FormulaVM> Models { get; set; } = new();
        public ContentControl Content;
      
        public FormulasViewModel(DiagramManager dmanager)
        {
            DiagramManager Diagram = dmanager;
            Name = dmanager.Name;
            var mod = dmanager.АllNodes;
            if (mod != null)
                foreach (var mod1 in mod)
                {
                    if (mod1 is FunkNodeModel)
                        Models.Add(new FormulaVM((FunkNodeModel)mod1));
                }

            GenerateActualView();

            //   UpdateMath = ReactiveCommand.Create<Unit>(u => updatemod());

        }

        public Formulas GenerateActualView()
        {
            Formulas m = new Formulas();
            m.DataContext = this;
            return m;
        }

    }
}
