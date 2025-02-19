using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.PageItems;

[HtmlTargetElement("page-heading")]
public class PageHeadingTagHelper : TagHelper
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // Capture the inner content
        var childContent = await output.GetChildContentAsync();

        // Wrap it inside an H2 element
        output.TagName = "h2";  // Change <page-heading> to <h2>
        output.Attributes.SetAttribute("class", "title");
        output.Content.SetHtmlContent(childContent);
    }
}