namespace ForresterModeller.Entities
{
    public interface IDiagramEntity
    {
        void VisitorAccept();
        string Name { get; set; }
        string TypeName { get;  }
    }
}