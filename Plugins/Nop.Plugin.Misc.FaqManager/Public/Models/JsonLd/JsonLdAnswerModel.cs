using Newtonsoft.Json;
using Nop.Web.Models.JsonLD;

namespace Nop.Plugin.Misc.FaqManager.Public.Models.JsonLd;

/// <summary>
/// Represents FAQ answer schema
/// </summary>
public partial record JsonLdAnswerModel : JsonLdModel
{
    #region Properties

    [JsonProperty("@type")]
    public static string Type => "Answer";

    [JsonProperty("text")]
    public string Text { get; set; }

    #endregion
}
