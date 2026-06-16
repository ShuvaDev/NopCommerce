using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record AddProductToFaqGroupModel : BaseNopEntityModel
{
    public int AssociatedToProductId { get; set; }
}