using System.Text.Json.Serialization;

namespace myapp.Models

{
    [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum RpgClass
        {
                Knight,
                Mage, 

                Cleric,
        }


}

