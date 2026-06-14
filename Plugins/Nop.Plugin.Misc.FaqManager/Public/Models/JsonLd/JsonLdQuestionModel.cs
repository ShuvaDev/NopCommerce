using Newtonsoft.Json;
using Nop.Web.Models.JsonLD;

namespace Nop.Plugin.Misc.FaqManager.Public.Models.JsonLd;

/// <summary>
/// Represents FAQ question schema
/// </summary>
public partial record JsonLdQuestionModel : JsonLdModel
{
    #region Properties

    [JsonProperty("@type")]
    public static string Type => "Question";

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("acceptedAnswer")]
    public JsonLdAnswerModel AcceptedAnswer { get; set; }

    #endregion
}
