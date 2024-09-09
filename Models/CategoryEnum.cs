using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Org.OpenAPITools.Models
{
    /// <summary>
    /// Gets or Sets CategoryEnum
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CategoryEnum
    {

        /// <summary>
        /// Enum RiskMgmtPlanEnum for RiskMgmtPlan
        /// </summary>
        [EnumMember(Value = "RiskMgmtPlan")]
        RiskMgmtPlan = 1,

        /// <summary>
        /// Enum DHPCEnum for DHPC
        /// </summary>
        [EnumMember(Value = "DHPC")]
        DHPC = 2,

        /// <summary>
        /// Enum InfoHPEnum for InfoHP
        /// </summary>
        [EnumMember(Value = "InfoHP")]
        InfoHP = 3,

        /// <summary>
        /// Enum InfoPatientEnum for InfoPatient
        /// </summary>
        [EnumMember(Value = "InfoPatient")]
        InfoPatient = 4,

        /// <summary>
        /// Enum MultilanguagePIEnum for MultilanguagePI
        /// </summary>
        [EnumMember(Value = "MultilanguagePI")]
        MultilanguagePI = 5
    }
}
