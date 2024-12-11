namespace IdentityServerAspNetIdentity.Pages.Components.HtmlModal;

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