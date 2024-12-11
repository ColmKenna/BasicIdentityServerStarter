using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.RoleItem;

[HtmlTargetElement("selection-item", Attributes = "name, value")]
public class SelectionItemTagHelper : TagHelper
{
    public string Name { get; set; }
    public string Value { get; set; }
    public bool Checked { get; set; } = false;
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
       /*
        * <div class="check-selection-item">
              <input type="checkbox" name="User.Roles[]" value="USER" checked=""> 
              <label class="check-selection-label" for="User_Roles">USER</label>
          </div>
        */
       
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "check-selection-item");
        
        TagBuilder inputTagBuilder = new TagBuilder("input");
        inputTagBuilder.Attributes.Add("type", "checkbox");
        inputTagBuilder.Attributes.Add("name", Name);
        inputTagBuilder.Attributes.Add("value", Value);
        if (Checked)
        {
            inputTagBuilder.Attributes.Add("checked", "");
        }
        
        TagBuilder labelTagBuilder = new TagBuilder("label");
        labelTagBuilder.Attributes.Add("class", "check-selection-label");
        labelTagBuilder.Attributes.Add("for", Name);
        labelTagBuilder.InnerHtml.Append(Value);
        
        output.Content.AppendHtml(inputTagBuilder);
        output.Content.AppendHtml(labelTagBuilder);
        
    }
    
}