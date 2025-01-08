
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityServerAspNetIdentity.Pages.Components.HtmlSelectionDialog;

public class HtmlSelectionViewModel
{
    
    
    public string ModalId { get; set; }
    public string ModalLabel => $"{ModalId}Label";
    public string ModalTitle { get; set; }
    public string Prompt { get; set; }
    public string ConfirmButtonText { get; set; }
    public string CancelButtonText { get; set; }
    public string ConfirmButtonFunction { get; set; }
    public string SelectionId { get; set; }
    public string SelectedValue { get;  set; }

    public List<SelectListItem> Items { get; set; }
    

    public HtmlSelectionViewModel(
        string modalId,
        string modalTitle,
        string prompt,
        string confirmButtonText,
        string cancelButtonText,
        string confirmButtonFunction,
        string selectionId,
        string selectedValue,
        List<SelectListItem> items)
    {
        // Assign passed-in parameters to the corresponding properties
        ModalId = modalId;
        ModalTitle = modalTitle;
        Prompt = prompt;
        ConfirmButtonText = confirmButtonText;
        CancelButtonText = cancelButtonText;
        ConfirmButtonFunction = confirmButtonFunction;
        SelectionId = selectionId;
        SelectedValue = selectedValue;
        Items = items;
    }
}