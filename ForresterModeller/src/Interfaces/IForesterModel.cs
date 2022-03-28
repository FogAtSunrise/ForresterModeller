using System.Collections.ObjectModel;
using System.Text.Json.Nodes;
using ForresterModeller.src.Pages.Properties;

namespace ForresterModeller.src.Nodes.Models
{
    public interface IForesterModel : IJSONSerializable, IPropertyChangable
    {
        public string Description { get; set; }
        /// <summary>
        /// Литерал, обозначающий тип узла
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Уникальный идентификатор модели
        /// </summary>
        public string Id { get; set; }

    }
}