using System.Text.Json.Nodes;

namespace ForresterModeller.src.Nodes.Models
{
    public interface IJSONSerializable
    {
        public abstract JsonObject ToJSON();
        public abstract void FromJSON(JsonObject obj);
    }
}