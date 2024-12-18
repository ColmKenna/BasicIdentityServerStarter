using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.Fields;

[HtmlTargetElement("field-container", Attributes = "label")]
public class FieldContainerTagHelper : TagHelper
{
    public FieldContainerTagHelper(IHtmlGenerator generator)
    {
        Generator = generator;
    }

    public string Label { get; set; }
    public string Value { get; set; }

    // for 
    public ModelExpression For { get; set; }

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    /// <summary>
    /// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="LabelTagHelper"/>'s output.
    /// </summary>
    protected IHtmlGenerator Generator { get; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "p"; // Replace the original tag with a <p> tag
        output.TagMode = TagMode.StartTagAndEndTag; // Ensure it renders a complete tag
        output.Attributes.SetAttribute("class", "field-container");


        if (For != null)
        {
            output.Content.SetHtmlContent($@"
                <strong>{Label}</strong>");

            if (For.Metadata.ModelType == typeof(bool) || For.Metadata.ModelType == typeof(bool?))
            {

                var checkedAttribute = For.Model != null && (bool)For.Model ? "checked" : "";
                var checkLabelTagHelper = Generator.GenerateCheckBox(
                    ViewContext,
                    For.ModelExplorer,
                    For.Name,
                    isChecked: For.Model != null && (bool)For.Model,

                    htmlAttributes: new Dictionary<string, object>
                    {
                        { "type", "checkbox" },
                        { "disabled", "disabled" }
                    }
                );
                output.Content.AppendHtml("<span>");
                output.Content.AppendHtml(checkLabelTagHelper);
                output.Content.AppendHtml("</span>");
            }
            else
            {
                Value = For.Model?.ToString() ?? "";

                var inputTagHelper = Generator.GenerateTextBox(
                    ViewContext,
                    For.ModelExplorer,
                    For.Name,
                    Value,
                    null,
                    new { @class = "form-control" }
                );
                output.Content.AppendHtml("<span>");
                output.Content.AppendHtml(inputTagHelper);
                output.Content.AppendHtml("</span>");

            }
        }
        else
        {
            output.Content.SetHtmlContent($@"
                <strong>{Label}</strong> <span>{Value}</span>
            ");
        }
    }
}