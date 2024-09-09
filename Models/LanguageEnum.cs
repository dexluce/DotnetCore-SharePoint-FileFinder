using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Org.OpenAPITools.Models
{
    /// <summary>
    /// Gets or Sets LanguagesEnum
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LanguageEnum
    {

        /// <summary>
        /// Enum FREnum for FR
        /// </summary>
        [EnumMember(Value = "FR")]
        FR = 1,

        /// <summary>
        /// Enum ENEnum for EN
        /// </summary>
        [EnumMember(Value = "EN")]
        EN = 2,

        /// <summary>
        /// Enum DEEnum for DE
        /// </summary>
        [EnumMember(Value = "DE")]
        DE = 3,

        /// <summary>
        /// Enum ITEnum for IT
        /// </summary>
        [EnumMember(Value = "IT")]
        IT = 4
    }
}
