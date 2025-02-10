#nullable enable
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.FormRow;

[HtmlTargetElement("form-row-template", Attributes = "root,for")]
public class FormRowTemplateTagHelper : TagHelper
{

    public string Label { get; set; } = string.Empty;

    [HtmlAttributeName("root")]
    public ModelExpression? Root { get; set; } = null;

    [HtmlAttributeName("for")]
    public ModelExpression For { get; set; } = null!;


    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public bool Hidden { get; set; } = false;
    
    protected IHtmlGenerator Generator { get; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var name = "";

        name = Root != null 
            ? $"{Root.Name}[replace_with_index].{For.Name}" 
            : For.Name;

        For = new ModelExpression(name, For.ModelExplorer);

        FormRowTagHelper tag = new FormRowTagHelper(Generator)
        {
            For = For, 
            Label = Label,
            ViewContext = ViewContext,
            Hidden = Hidden
        };
        
        tag.Process(context, output);
    }

    public FormRowTemplateTagHelper(IHtmlGenerator generator)
    {
        Generator = generator;
    }
}