using System.Windows.Navigation;

namespace WPFtest1.Entities
{
    public class DiagramConstant: DiagramElement
    {
        public override string Name { get ; set; }
        public override string TypeName => "Константа";
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