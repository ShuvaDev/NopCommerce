using Newtonsoft.Json;
using Nop.Web.Models.JsonLD;

namespace Nop.Plugin.Misc.FaqManager.Public.Models.JsonLd;

/// <summary>
/// Represents FAQ page schema
/// </summary>
public partial record JsonLdFaqPageModel : JsonLdModel
{
    #region Ctor

    public JsonLdFaqPageModel()
    {
        MainEntity = new List<JsonLdQuestionModel>();
    }

    #endregion

    #region Properties

    [JsonProperty("@context")]
    public static string Context => "https://schema.org";

    [JsonProperty("@type")]
    public static string Type => "FAQPage";

    [JsonProperty("mainEntity")]
    public IList<JsonLdQuestionModel> MainEntity { get; set; }

    #endregion
}
