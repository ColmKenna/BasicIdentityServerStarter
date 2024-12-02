using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.Fields;

[HtmlTargetElement("field-container", Attributes = "label, value")]
public class FieldContainerTagHelper : TagHelper
{
    public string Label { get; set; }
    public string Value { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "p"; // Replace the original tag with a <p> tag
        output.TagMode = TagMode.StartTagAndEndTag; // Ensure it renders a complete tag
        output.Attributes.SetAttribute("class", "field-container");

        // Set the content
        output.Content.SetHtmlContent($@"
                <strong>{Label}</strong> {Value}
            ");
    }
}