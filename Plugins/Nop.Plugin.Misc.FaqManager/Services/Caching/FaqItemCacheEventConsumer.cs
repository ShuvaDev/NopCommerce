using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Services.Caching;

namespace Nop.Plugin.Misc.FaqManager.Services.Caching;

/// <summary>
/// Represents a FAQ item cache event consumer
/// </summary>
public class FaqItemCacheEventConsumer : CacheEventConsumer<FaqItem>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(FaqItem entity)
    {
        await RemoveByPrefixAsync(FaqManagerDefaults.FaqPrefixCacheKey);
    }
}
