using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityServerAspNetIdentity.Pages.Components.HtmlSelectionDialog;

public class HtmlSelectionDialogViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(HtmlSelectionViewModel model)
    {
        return View(model);
    }
    //
    // public IViewComponentResult Invoke(
    //     string modalId,
    //     string modalTitle,
    //     string prompt,
    //     string confirmButtonText,
    //     string cancelButtonText,
    //     string confirmButtonFunction,
    //     string selectionId,
    //     string selectedValue,
    //     List<SelectListItem> items)
    // {
    //     var model = new HtmlSelectionViewModel(
    //         modalId,
    //         modalTitle,
    //         prompt,
    //         confirmButtonText,
    //         cancelButtonText,
    //         confirmButtonFunction,
    //         selectionId,
    //         selectedValue,
    //         items
    //     );
    //
    //     return View(model);
    // }
}