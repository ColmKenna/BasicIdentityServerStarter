using System.Text;
using IdentityServerAspNetIdentity.Pages.Components.HtmlModal;
using IdentityServerAspNetIdentity.TagHelpers.RoleItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.Selections;

[HtmlTargetElement("selection-container", Attributes = "id,source,selected,name")]
public class SelectionContainerTagHelper : TagHelper
{
    private readonly IViewComponentHelper _viewComponentHelper;
    
    // ViewContext
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }
    
    
    public string Id { get; set; }
    
    /// <summary>
    /// The source of all possible items (e.g., all roles).
    /// </summary>
    public IEnumerable<string> Source { get; set; }

    /// <summary>
    /// The currently selected items (e.g., the user's assigned roles).
    /// </summary>
    public IEnumerable<string> Selected { get; set; }

    /// <summary>
    /// The name attribute to be used for the selection inputs (e.g., "User.Roles[]").
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Whether to allow adding new items not present in Source.
    /// </summary>
    [HtmlAttributeName("allow-add")]
    public bool AllowAdd { get; set; }
    
    public string Title { get; set; } = String.Empty;

    
    
    public SelectionContainerTagHelper(IViewComponentHelper viewComponentHelper)
    {
        _viewComponentHelper = viewComponentHelper;
    }
    
    
    
    public override async  Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (Title == String.Empty)
        {
            Title = Name;
        }
        
        output.TagName = "div";
        output.Attributes.SetAttribute("id", Id);
        output.Attributes.SetAttribute("class", "check-selection-container");


        var childContext = new TagHelperContext(
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: Guid.NewGuid().ToString("N")
        );

        if (Source != null)
        {
            
            foreach (var item in Source)
            {
                bool isChecked = Selected?.Contains(item) == true;
                
                var childOutput = await TagHelperOutput(Name,item, isChecked, childContext);

                // Append the child's rendered output to the parent output
                output.Content.AppendHtml(childOutput);
                
                 
                
            }
        }
        
        

        if (AllowAdd)
        {

            var modalId = Id + "Modal";
            var templateId = Id + "Template";

            output.Content.AppendHtml(
                $@"<button type=""button"" class=""secondary-button"" data-ck-modal-target=""{modalId}"">Add {Title}</button>");
            output.Content.AppendHtml($@"<template id=""{templateId}"">");

            var selectionItemTagHelper = new SelectionItemTagHelper()
            {
                Name = Name,
                Value = "",
                Checked = true
            };

            
            var childOutput = await TagHelperOutput(Name,"", true, childContext);
            output.Content.AppendHtml(childOutput);
            
            output.Content.AppendHtml($@"</template>");









            var function = $@"addNewItem('{Title}','new{Id}Input','{Id}Template','{Id}')";



            var newRoleModalViewModel = new ModalViewModel(
                Id + "Modal",
                "Add New " + Title,
                "Enter the following information:",
                "Add " + Title,
                "Close",
                function,
                new ModalInputs($"new{Id}Input", Title)
            );

            (_viewComponentHelper as IViewContextAware)?.Contextualize(ViewContext);

            var result = await _viewComponentHelper.InvokeAsync(typeof(HtmlModalViewComponent),
                newRoleModalViewModel);

            output.PostContent.SetHtmlContent(result);

        }
    }

    private async Task<TagHelperOutput> TagHelperOutput(string name,  string item, bool isChecked, TagHelperContext childContext)
    {
        var selectionItemTagHelper = new SelectionItemTagHelper()
        {
            Name = name,
            Value = item,
            Checked = isChecked
        };
                
        var childOutput = new TagHelperOutput(
            "selection-item", 
            attributes: new TagHelperAttributeList
            {
                new TagHelperAttribute("name", Name),
                new TagHelperAttribute("value", item),
                new TagHelperAttribute("checked", isChecked.ToString().ToLower())
            },
            getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent())
        );
                
        await selectionItemTagHelper.ProcessAsync(childContext, childOutput);
        return childOutput;
    }

    private TagHelperOutput CreateChildOutput(string name, string item, bool isChecked)
    {
        var selectionItemTagHelper = new SelectionItemTagHelper()
        {
            Name = name,
            Value = item,
            Checked = isChecked
        };

        var childOutput = new TagHelperOutput(
            "selection-item", 
            attributes: new TagHelperAttributeList
            {
                new TagHelperAttribute("name", name),
                new TagHelperAttribute("value", item),
                new TagHelperAttribute("checked", isChecked.ToString().ToLower())
            },
            getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent())
        );

        return childOutput;
    }
    
    
    
}