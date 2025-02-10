#nullable enable
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.FormRow;

[HtmlTargetElement("form-row-text-input", Attributes = "asp-for" )]
[HtmlTargetElement("form-template-row-text-input", Attributes = "asp-for,base-type" )]
public class FormRowTextInputTagHelper : TagHelper
{
    private readonly IHtmlGenerator _htmlGenerator;

    public FormRowTextInputTagHelper(IHtmlGenerator htmlGenerator)
    {
        _htmlGenerator = htmlGenerator;
    }

    [HtmlAttributeName("asp-for")] public ModelExpression For { get; set; }

    [HtmlAttributeName("base-type")] public ModelExpression? BaseType { get; set; }
    

    [HtmlAttributeNotBound] [ViewContext] public ViewContext ViewContext { get; set; }

    [HtmlAttributeName("class")] public string CssClass { get; set; }

    [HtmlAttributeName("label-class")] public string LabelCssClass { get; set; }

    [HtmlAttributeName("input-class")] public string InputCssClass { get; set; }

    // id
    [HtmlAttributeName("id")] public string Id { get; set; }

    [HtmlAttributeName("input-attributes", DictionaryAttributePrefix = "input-")]
    public IDictionary<string, object> InputAttributes { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// If true, renders only a hidden input with the model's current value.
    /// </summary>
    [HtmlAttributeName("hidden")]
    public bool Hidden { get; set; }


    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        
        var name = "";

        name = BaseType != null 
            ? $"{BaseType.Name}[replace_with_index].{For.Name}" 
            : For.Name;

        For = new ModelExpression(name, For.ModelExplorer);

        
        
        if (string.IsNullOrEmpty(Id))
        {
            Id = For.Name.Replace(".", "_");
        }

        var divId = Id?.Trim() + "_Container";

        output.Attributes.SetAttribute("id", divId);

        if (For == null) throw new InvalidOperationException("The 'asp-for' attribute is required.");

        if (Hidden)
        {
            output.TagName = null;
            output.Attributes.Clear();
            output.Content.Clear();

            var modelValue = For.Model?.ToString() ?? string.Empty;
            var hiddenInput = new TagBuilder("input");
            hiddenInput.Attributes["type"] = "hidden";
            hiddenInput.Attributes["name"] = For.Name;
            hiddenInput.Attributes["value"] = modelValue;

            output.Content.AppendHtml(hiddenInput);
            return;
        }

        // Set up the outer div
        output.TagName = "div";
        var divClass = $"form-row {CssClass}".Trim();
        output.Attributes.SetAttribute("class", divClass);

        // Generate the label
        var labelClass = $"form-label {LabelCssClass}".Trim();
        var labelHtmlAttributes = new Dictionary<string, object>
        {
            { "class", labelClass }
        };


        var labelTag = _htmlGenerator.GenerateLabel(
            viewContext: ViewContext,
            modelExplorer: For.ModelExplorer,
            expression: For.Name,
            labelText: null, // Use the default label text from model metadata
            htmlAttributes: labelHtmlAttributes);

        var labelText = labelTag.InnerHtml;
        output.Content.AppendHtml(labelTag);

        // Generate the input element
        var inputClass = $"form-input {InputCssClass}".Trim();
        var inputAttributes = new Dictionary<string, object>(InputAttributes)
        {
            { "class", inputClass }
        };

        var inputTag = _htmlGenerator.GenerateTextBox(
            viewContext: ViewContext,
            modelExplorer: For.ModelExplorer,
            expression: For.Name,
            value: For.Model, // Let the generator handle the value
            format: null,
            htmlAttributes: inputAttributes);

        output.Content.AppendHtml(inputTag);
    }
}