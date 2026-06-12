using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.FaqManager.Domain;

namespace Nop.Plugin.Misc.FaqManager.Data.Mapping.Builders;

/// <summary>
/// Represents a faq item entity builder
/// </summary>
public class FaqItemBuilder : NopEntityBuilder<FaqItem>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(FaqItem.Question)).AsString(1000).NotNullable()
            .WithColumn(nameof(FaqItem.Answer)).AsString(int.MaxValue).NotNullable()
            .WithColumn(nameof(FaqItem.DisplayOrder)).AsInt32().NotNullable()
            .WithColumn(nameof(FaqItem.Published)).AsBoolean().NotNullable()
            .WithColumn(nameof(FaqItem.FaqGroupId)).AsInt32().NotNullable().ForeignKey<FaqGroup>();
    }

    #endregion
}
