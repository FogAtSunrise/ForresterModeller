namespace ForresterModeller.Entities
{
    public abstract class DiagramElement : IDiagramEntity
    {
        public abstract string Code();
        public abstract void VisitorAccept();
        public abstract string Name { get; set; }
        public abstract string TypeName { get;  }
    }
}