namespace WPFtest1.Entities
{
    public interface IDiagramEntity
    {
        void VisitorAccept();
        string Name { get; set; }
        string TypeName { get;  }
    }
}