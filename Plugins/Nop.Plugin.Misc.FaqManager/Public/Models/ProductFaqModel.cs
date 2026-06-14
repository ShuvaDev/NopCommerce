namespace Nop.Plugin.Misc.FaqManager.Public.Models;

public class ProductFaqModel
{

    public string JsonLd { get; set; }
    public bool ShowFaqCount { get; set; }

    public int FaqCount { get; set; }

    public IList<FaqItemModel> Items { get; set; } = [];
}
