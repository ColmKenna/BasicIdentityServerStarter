using Microsoft.AspNetCore.Mvc;

namespace IdentityServerAspNetIdentity.Pages.Components.HtmlModal;

public class HtmlModalViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ModalViewModel model)
    {
        return View(model);
    }
}

public class ModalViewModel
{
    
    
    public string ModalId { get; set; }
    public string ModalLabel => $"{ModalId}Label";
    public string ModalTitle { get; set; }
    public string Prompt { get; set; }
    public string ConfirmButtonText { get; set; }
    public string CancelButtonText { get; set; }
    public List<ModalInputs> Inputs { get; set; }
    public string ConfirmButtonFunction { get; set; }  // New Property for JavaScript Function

    public ModalViewModel(
        string modalId,
        string modalTitle,
        string prompt,
        string confirmButtonText,
        string cancelButtonText,
        string confirmButtonFunction,
        params ModalInputs[] inputs)
    {
        // Assign passed-in parameters to the corresponding properties
        ModalId = modalId;
        ModalTitle = modalTitle;
        Prompt = prompt;
        ConfirmButtonText = confirmButtonText;
        CancelButtonText = cancelButtonText;
        ConfirmButtonFunction = confirmButtonFunction;
        Inputs = new List<ModalInputs>(inputs);
    }
}

public class ModalInputs
{
    public ModalInputs(string inputId, string inputLabel)
    {
        InputId = inputId;
        InputLabel = inputLabel;
    }

    public ModalInputs(string inputId, string inputLabel, string inputType)
    {
        InputId = inputId;
        InputLabel = inputLabel;
        InputType = inputType;
    }

    public ModalInputs(string inputId, string inputLabel, string inputType, string inputPlaceholder)
    {
        InputId = inputId;
        InputLabel = inputLabel;
        InputType = inputType;
        InputPlaceholder = inputPlaceholder;
    }

    public ModalInputs(string inputId, string inputLabel, string inputType, string inputPlaceholder,
        string inputDefaultValue)
    {
        InputId = inputId;
        InputLabel = inputLabel;
        InputType = inputType;
        InputPlaceholder = inputPlaceholder;
        InputDefaultValue = inputDefaultValue;
    }
    public ModalInputs(string inputId, string inputLabel, string inputType, string inputPlaceholder, bool required)
    {
        InputId = inputId;
        InputLabel = inputLabel;
        InputType = inputType;
        InputPlaceholder = inputPlaceholder;
        Required = required;
    }
    public string InputId { get; set; }

    public string InputLabel { get; set; }

    public string? InputType { get; set; }

    public string? InputDefaultValue { get; set; }
   
    public string? InputPlaceholder { get; set; }
    
    public bool? Required { get; set; }
    
}