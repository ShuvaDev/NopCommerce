using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.FaqManager.Domain;

namespace Nop.Plugin.Misc.FaqManager.Data.Mapping.Builders;

/// <summary>
/// Represents a faq group entity builder
/// </summary>
public class FaqGroupBuilder : NopEntityBuilder<FaqGroup>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(FaqGroup.Name)).AsString(400).NotNullable()
            .WithColumn(nameof(FaqGroup.ProductId)).AsInt32().ForeignKey<Product>()
            .WithColumn(nameof(FaqGroup.DisplayOrder)).AsInt32().NotNullable()
            .WithColumn(nameof(FaqGroup.Published)).AsBoolean().NotNullable();
    }

    #endregion
}
