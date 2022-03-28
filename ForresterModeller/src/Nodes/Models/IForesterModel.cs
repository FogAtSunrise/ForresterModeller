using System.Collections.ObjectModel;
using System.Text.Json.Nodes;
using ForresterModeller.src.Pages.Properties;

namespace ForresterModeller.src.Nodes.Models
{
    public interface IForesterModel
    {
        public static string type="";
        public string Description { get; set; }
        /// <summary>
        /// Литерал, обозначающий тип узла
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Уникальный идентификатор модели
        /// </summary>
        public string Id { get; set; }
        public abstract ObservableCollection<Property> GetProperties();
        public abstract JsonObject ToJSON();
        public abstract bool FromJSON(JsonObject obj);

    }
}