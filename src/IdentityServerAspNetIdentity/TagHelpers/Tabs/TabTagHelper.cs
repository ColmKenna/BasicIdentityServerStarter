using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.Tabs;


[HtmlTargetElement("tab")]
public class TabTagHelper : TagHelper
{
    public string Id { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // Initialize shared context for headers and contents
        context.Items["TabHeaders"] = new List<string>();
        context.Items["TabContents"] = new List<string>();

        // Process child tags
        var childContent = await output.GetChildContentAsync();

        // Retrieve aggregated headers and contents
        var headers = context.Items["TabHeaders"] as List<string> ?? new List<string>();
        var contents = context.Items["TabContents"] as List<string> ?? new List<string>();

        // Render the parent container
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "tab-container");

        output.Content.SetHtmlContent($@"
            <div class='tab-headers'>
                {string.Join("\n", headers)}
            </div>
            <div class='tabs'>
                {string.Join("\n", contents)}
            </div>
        ");
    }
}