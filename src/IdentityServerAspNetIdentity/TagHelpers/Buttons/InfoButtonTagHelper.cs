using Microsoft.AspNetCore.Razor.TagHelpers;
namespace IdentityServerAspNetIdentity.TagHelpers.Buttons;

[HtmlTargetElement("info-button")]
    public class InfoButtonTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            output.Attributes.SetAttribute("class", "info-button");
            var childContent = await output.GetChildContentAsync();
            var content = childContent.GetContent();
            output.Content.SetHtmlContent(content);
        }
    }