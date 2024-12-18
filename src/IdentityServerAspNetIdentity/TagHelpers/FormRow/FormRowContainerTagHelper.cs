#nullable enable
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.FormRow;

[HtmlTargetElement("form-row-container")]
public class FormRowContainerTagHelper : TagHelper
{
    public FormRowContainerTagHelper(IHtmlGenerator generator)
    {
        Generator = generator;
    }


    public string Label { get; set; } = string.Empty;
    IHtmlGenerator Generator { get; }
    [HtmlAttributeNotBound] [ViewContext] public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "form-row");

        var labelTag = new TagBuilder("label");
        labelTag.AddCssClass("form-label");
        labelTag.InnerHtml.Append(Label);
        output.PreContent.AppendHtml(labelTag);
        
        
        output.Content.AppendHtml(output.GetChildContentAsync().Result);
        
        
        

    }
}