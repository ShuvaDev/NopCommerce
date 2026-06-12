using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;

namespace Nop.Plugin.Misc.FaqManager.Domain;

/// <summary>
/// Represents a FAQ group assigned to a product
/// </summary>
public class FaqGroup : BaseEntity, ILocalizedEntity
{
    /// <summary>
    /// Group name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Product identifier
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Whether the group is published
    /// </summary>
    public bool Published { get; set; }

    /// <summary>
    /// Display order
    /// </summary>
    public int DisplayOrder { get; set; }
}