using Nop.Core;
using Nop.Plugin.Misc.FaqManager.Domain;

namespace Nop.Plugin.Misc.FaqManager.Services;

public interface IFaqGroupService
{
    Task<FaqGroup> GetFaqGroupByIdAsync(int faqGroupId);

    Task<IPagedList<FaqGroup>> GetAllFaqGroupsAsync(
        string name = null,
        int productId = 0,
        bool? published = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue);

    Task<IList<FaqGroup>> GetFaqGroupsByProductIdAsync(
        int productId,
        bool showHidden = false);

    Task<FaqGroup> GetPublishedFaqGroupByProductIdAsync(
        int productId);

    Task InsertFaqGroupAsync(FaqGroup faqGroup);

    Task UpdateFaqGroupAsync(FaqGroup faqGroup);

    Task DeleteFaqGroupAsync(FaqGroup faqGroup);
}