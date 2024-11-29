using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.Links;

public abstract class BaseLinkTagHelper : TagHelper
{
    public string Href { get; set; } = "#";
    public Size Size { get; set; } = Size.Medium;

    protected abstract string ButtonClass { get; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";
        output.Attributes.SetAttribute("href", Href);

        var classStringBuilder = new StringBuilder();
        if (output.Attributes.ContainsName("class"))
        {
            classStringBuilder.Append(output.Attributes["class"].Value.ToString());
        }

        classStringBuilder.Append(ButtonClass).Append(" ");
        classStringBuilder.Append(Size switch
        {
            Size.Small => "btn-small ",
            Size.Large => "btn-lg ",
            _ => ""
        });

        output.Attributes.SetAttribute("class", classStringBuilder.ToString());

        var childContent = await output.GetChildContentAsync();
        var content = childContent.GetContent();

        output.Content.SetHtmlContent(content);
    }
}