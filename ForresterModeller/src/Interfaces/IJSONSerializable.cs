using System.Text.Json.Nodes;

namespace ForresterModeller.src.Interfaces
{
    public interface IJSONSerializable
    {
        public abstract JsonObject ToJSON();
        public abstract void FromJSON(JsonObject obj);
    }
}