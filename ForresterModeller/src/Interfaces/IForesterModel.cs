using ForresterModeller.src.Nodes.Viters;

namespace ForresterModeller.src.Interfaces
{
    public interface IForesterModel : IJSONSerializable, IPropertyOwner
    {
        public static string type="";
        public string Description { get; set; }
        /// <summary>
        /// Литерал, обозначающий тип узла
        /// </summary>
        public string TypeName { get; }

        public T AcceptViseter<T>(INodeViseters<T> viseter);

        /// <summary>
        /// Уникальный идентификатор модели
        /// </summary>
        public string Id { get; set; }
    }
}