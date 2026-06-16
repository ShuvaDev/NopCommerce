using Nop.Core;
using Nop.Plugin.Misc.FaqManager.Domain;

namespace Nop.Plugin.Misc.FaqManager.Services;
public interface IFaqItemService
{
    Task<FaqItem?> GetFaqItemByIdAsync(int faqItemId);

    Task<IPagedList<FaqItem>> GetAllFaqItemsAsync(
        string question = null,
        int faqGroupId = 0,
        bool? published = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue);

    Task<IPagedList<FaqItem>> GetFaqItemsAsync(
        int faqGroupId,
        int pageIndex = 0,
        int pageSize = int.MaxValue);

    Task<IList<FaqItem>> GetFaqItemsByGroupIdAsync(
        int faqGroupId,
        bool showHidden = false);

    Task<IList<FaqItem>> GetPublishedFaqItemsByGroupIdAsync(
        int faqGroupId);

    Task InsertFaqItemAsync(FaqItem faqItem);

    Task UpdateFaqItemAsync(FaqItem faqItem);

    Task DeleteFaqItemAsync(FaqItem faqItem);
}
