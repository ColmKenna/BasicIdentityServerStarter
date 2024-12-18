namespace IdentityServerAspNetIdentity.Pages.Components.HtmlModal;

public class ModalViewModel
{
    
    
    public string ModalId { get; set; }
    public string ModalLabel => $"{ModalId}Label";
    public string ModalTitle { get; set; }
    public string Prompt { get; set; }
    public string ConfirmButtonText { get; set; }
    public string CancelButtonText { get; set; }
    public List<ModalInputs> Inputs { get; set; }
    public string ConfirmButtonFunction { get; set; }  

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