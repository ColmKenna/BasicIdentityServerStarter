#nullable enable
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.FormRow;

[HtmlTargetElement("form-row-dropdown")]
public class FormRowDropdownTagHelper : TagHelper
{
    private readonly IHtmlGenerator _htmlGenerator;

    public FormRowDropdownTagHelper(IHtmlGenerator htmlGenerator)
    {
        _htmlGenerator = htmlGenerator;
    }

    [HtmlAttributeName("asp-for")]
    public ModelExpression For { get; set; }

    [HtmlAttributeName("asp-items")]
    public IEnumerable<SelectListItem> Items { get; set; }

    [HtmlAttributeNotBound] [ViewContext] public ViewContext ViewContext { get; set; }

    [HtmlAttributeName("label")]
    public string Label { get; set; }

    [HtmlAttributeName("option-label")]
    public string OptionLabel { get; set; }

    [HtmlAttributeName("id")]
    public string Id { get; set; }

    [HtmlAttributeName("class")]
    public string CssClass { get; set; }

    [HtmlAttributeName("label-class")]
    public string LabelCssClass { get; set; }

    [HtmlAttributeName("select-class")]
    public string SelectCssClass { get; set; }

    [HtmlAttributeName("select-attributes", DictionaryAttributePrefix = "select-")]
    public IDictionary<string,object> SelectAttributes { get; set; } = new Dictionary<string, object>();

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (For == null) throw new InvalidOperationException("The 'asp-for' attribute is required.");
        if (Items == null) throw new InvalidOperationException("The 'asp-items' attribute is required.");

        var divId = Id?.Trim();
        if (!string.IsNullOrEmpty(divId))
        {
            output.Attributes.SetAttribute("id", divId);
        }

        output.TagName = "div";

        var divClass = $"form-row {CssClass}".Trim();
        output.Attributes.SetAttribute("class", divClass);

        if (!string.IsNullOrEmpty(Label))
        {
            var labelClass = $"form-label {LabelCssClass}".Trim();
            var labelTag = _htmlGenerator.GenerateLabel(
                viewContext: ViewContext,
                modelExplorer: For.ModelExplorer,
                expression: For.Name,
                labelText: Label,
                htmlAttributes: new { @class = labelClass });

            output.Content.AppendHtml(labelTag);
        }

        var selectId = string.IsNullOrEmpty(divId) ? null : $"{divId}-select";
        var selectClass = $"form-input {SelectCssClass}".Trim();
        var selectAttributes = new Dictionary<string, object>(SelectAttributes)
        {
            { "class", selectClass }
        };

        if (!string.IsNullOrEmpty(selectId))
        {
            selectAttributes["id"] = selectId;
        }

        var selectTag = _htmlGenerator.GenerateSelect(
            viewContext: ViewContext,
            modelExplorer: For.ModelExplorer,
            expression: For.Name,
            optionLabel: OptionLabel,
            selectList: Items,
            currentValues: null,
            allowMultiple: false,
            htmlAttributes: selectAttributes);

        output.Content.AppendHtml(selectTag);
    }
}