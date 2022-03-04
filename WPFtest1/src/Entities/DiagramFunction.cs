using ForresterModeller.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFtest1.src.Entities
{
    class DiagramFunction : DiagramElement
    {
        public override string Name { get; set; }
        public override string TypeName => "Функция";
        private double Value { get; set; }
        public override string Code()
        {
            throw new System.NotImplementedException();
        }

        public override void VisitorAccept()
        {
            throw new System.NotImplementedException();
        }
    }
}
