#nullable enable
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorExtensions;

namespace IdentityServerAspNetIdentity.TagHelpers.FormRow;

[HtmlTargetElement("form-row-checkbox", Attributes = "asp-for")]
public class FormRowCheckboxTagHelper : TagHelper
{
    private readonly IHtmlGenerator _htmlGenerator;

    public FormRowCheckboxTagHelper(IHtmlGenerator htmlGenerator)
    {
        _htmlGenerator = htmlGenerator;
    }

    [HtmlAttributeName("asp-for")] public ModelExpression For { get; set; }


    [HtmlAttributeNotBound] [ViewContext] public ViewContext ViewContext { get; set; }

    [HtmlAttributeName("class")] public string CssClass { get; set; }

    [HtmlAttributeName("label-class")] public string LabelCssClass { get; set; }

    [HtmlAttributeName("input-class")] public string InputCssClass { get; set; }

    [HtmlAttributeName("id")] public string Id { get; set; }

    [HtmlAttributeName("input-attributes", DictionaryAttributePrefix = "input-")]
    public IDictionary<string, object> InputAttributes { get; set; } = new Dictionary<string, object>();

    
    [HtmlAttributeName("as-slider")] 
    public bool Slider { get; set; } = false; 

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (For == null) throw new InvalidOperationException("The 'asp-for' attribute is required.");

        if (string.IsNullOrEmpty(Id))
        {
            Id = For.Name.Replace(".", "_");
        }

        var divId = Id?.Trim() + "_Container";
        output.Attributes.SetAttribute("id", divId);

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
            labelText: null,
            htmlAttributes: labelHtmlAttributes);

        output.Content.AppendHtml(labelTag);

        // Generate the checkbox input element
        var inputClass = $"form-check-input {InputCssClass}".Trim();
        var inputAttributes = new Dictionary<string, object>(InputAttributes)
        {
            { "class", inputClass }
        };

        var checkboxTag = _htmlGenerator.GenerateCheckBox(
            viewContext: ViewContext,
            modelExplorer: For.ModelExplorer,
            expression: For.Name,
            isChecked: (bool?)For.Model ?? false,
            htmlAttributes: inputAttributes);
        
        
        var sliderClass = Slider ? "form-switch" : "form-check";
        TagBuilder checkboxWrappedTag;
            
        // append a span to  checkboxWrappedTag for the slider
        if (Slider)
        {
            checkboxWrappedTag =  checkboxTag.CreateWrappedTag("label", "form-switch" );
            var sliderSpan = new TagBuilder("span");
            sliderSpan.AddCssClass("form-slider");
            checkboxWrappedTag.InnerHtml.AppendHtml(sliderSpan);
            checkboxWrappedTag =  checkboxWrappedTag.CreateWrappedTag("div", "" );
        }
        else
        {
            checkboxWrappedTag =  checkboxTag.CreateWrappedTag("div", "form-input form-check" );
        }

        output.Content.AppendHtml(checkboxWrappedTag);
    }
}