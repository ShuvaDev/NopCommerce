using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.Manufacturer.Models;
using Nop.Services.Catalog;
using Nop.Services.Helpers;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.Manufacturer.Components;

public class ManufacturerListViewComponent : NopViewComponent
{
    #region Fields

    private readonly IManufacturerService _manufacturerService;
    protected readonly IPictureService _pictureService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IWebHelper _webHelper;


    #endregion

    #region Ctor
    public ManufacturerListViewComponent(
       IManufacturerService manufacturerService,
       IPictureService pictureService,
       IUrlRecordService urlRecordService,
       IWebHelper webHelper)
    {
        _manufacturerService = manufacturerService;
        _pictureService = pictureService;
        _urlRecordService = urlRecordService;
        _webHelper = webHelper;
    }

    #endregion

    #region Utilities

    private async Task<string> GetPictureUrlAsync(int pictureId)
    {
        if (pictureId == 0)
            return string.Empty;

        var url = await _pictureService.GetPictureUrlAsync(pictureId, targetSize: 120, showDefaultPicture: false) ?? "";
        return url;
    }

    #endregion

    #region Methods

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var manufacturers = await _manufacturerService.GetAllManufacturersAsync(
            pageSize: int.MaxValue);

        var model = new PublicInfoModel
        {
            ShowImages = true,
            ShowNames = true,
        };

        foreach (var manufacturer in manufacturers)
        {
            var picUrl = await GetPictureUrlAsync(manufacturer.PictureId);
            var seName = await _urlRecordService.GetSeNameAsync(manufacturer);

            model.Manufacturers.Add(new()
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                SeName = seName,
                PictureUrl = picUrl
            });
        }

        if (!model.Manufacturers.Any())
            return Content("");

        return View(
            "~/Plugins/Widgets.Manufacturer/Views/PublicInfo.cshtml",
            model
        );
    }

    #endregion
}
