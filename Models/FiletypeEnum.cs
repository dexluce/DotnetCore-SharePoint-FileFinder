using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Org.OpenAPITools.Models
{
    /// <summary>
    /// Gets or Sets File type Enum
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FiletypeEnum
    {

        /// <summary>
        /// Enum BLOB for BLOB
        /// </summary>
        [EnumMember(Value = "BLOB")]
        BLOB = 1,

        /// <summary>
        /// Enum PDF for PDF
        /// </summary>
        [EnumMember(Value = "PDF")]
        PDF = 2,

        /// <summary>
        /// Enum LINK for LINK
        /// </summary>
        [EnumMember(Value = "LINK")]
        LINK = 3,

        /// <summary>
        /// Enum QR for QR
        /// </summary>
        [EnumMember(Value = "QR")]
        QR = 4,
    }
}
