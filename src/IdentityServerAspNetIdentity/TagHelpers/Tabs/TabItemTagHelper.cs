using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.Tabs;

[HtmlTargetElement("tab-item", ParentTag = "tab")]
public class TabItemTagHelper : TagHelper
{
    [HtmlAttributeName("id")]
    public string Id { get; set; }

    [HtmlAttributeName("selected")]
    public bool Selected { get; set; }
    
    [HtmlAttributeName("heading")]
    public string Heading { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var content = await output.GetChildContentAsync();
        var sb = new StringBuilder();

        sb.Append($"<input class=\"tabs-panel-input\" name=\"tabs\" type=\"radio\" id=\"{Id}\" {(Selected ? "checked=\"checked\"" : "")}/>");
        sb.Append($"<label class=\"tab-heading\" for=\"{Id}\">{Heading}</label>");
        sb.Append($"<div class=\"panel\"><div class=\"panel-content\">{content.GetContent()}</div></div>");

        output.TagName = null; // Remove the original <tab-item> tag
        output.Content.SetHtmlContent(sb.ToString());
    }
}