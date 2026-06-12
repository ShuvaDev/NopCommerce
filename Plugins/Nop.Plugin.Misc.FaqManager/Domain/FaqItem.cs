using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Misc.FaqManager.Domain;

/// <summary>
/// Represents a FAQ item (question & answer)
/// </summary>
public class FaqItem : BaseEntity, ILocalizedEntity
{
    /// <summary>
    /// Parent FAQ group identifier
    /// </summary>
    public int FaqGroupId { get; set; }

    /// <summary>
    /// Question
    /// </summary>
    public string Question { get; set; }

    /// <summary>
    /// Answer (HTML content)
    /// </summary>
    public string Answer { get; set; }

    /// <summary>
    /// Whether the item is published
    /// </summary>
    public bool Published { get; set; }

    /// <summary>
    /// Display order
    /// </summary>
    public int DisplayOrder { get; set; }
}
