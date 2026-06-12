using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Services.Caching;

namespace Nop.Plugin.Misc.FaqManager.Services.Caching;

/// <summary>
/// Represents a FAQ group cache event consumer
/// </summary>
public class FaqGroupCacheEventConsumer : CacheEventConsumer<FaqGroup>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(FaqGroup entity)
    {
        await RemoveByPrefixAsync(FaqManagerDefaults.FaqPrefixCacheKey);
    }
}
