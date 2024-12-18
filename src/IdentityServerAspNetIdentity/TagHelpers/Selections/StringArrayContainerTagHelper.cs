using IdentityServerAspNetIdentity.Pages.Components.HtmlModal;
using IdentityServerAspNetIdentity.TagHelpers.FormRow;
using IdentityServerAspNetIdentity.TagHelpers.RoleItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.Selections;

[HtmlTargetElement("string-array-container", Attributes = "id,current,name,title")]
public class StringArrayContainerTagHelper : TagHelper
{

    // generator
    private IHtmlGenerator Generator { get; }
    
    private readonly IViewComponentHelper _viewComponentHelper;


    // ViewContext
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }

    [HtmlAttributeName("id")]
    public string Id { get; set; }

    [HtmlAttributeName("current")]
    public IEnumerable<string> Current { get; set; }

    [HtmlAttributeName("name")]
    public string Name { get; set; }

    [HtmlAttributeName("title")]
    public string Title { get; set; }

    [HtmlAttributeName("allow-add")]
    public bool AllowAdd { get; set; }

    public StringArrayContainerTagHelper(IViewComponentHelper viewComponentHelper, IHtmlGenerator generator)
    {
        _viewComponentHelper = viewComponentHelper;
        Generator = generator;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (Title == String.Empty)
        {
            Title = Name;
        }

        output.TagName = "div";
        var titleDiv = new TagBuilder("div");
        titleDiv.AddCssClass("form-row text-form-row");
        
        // add a label for the title
        var label = new TagBuilder("label");
        label.AddCssClass("form-label");
        label.InnerHtml.Append(Title);
        titleDiv.InnerHtml.AppendHtml(label);
        
        //output.Content.AppendHtml(titleDiv);
        
        // add a div for the items
        var itemsDiv = new TagBuilder("div");
        itemsDiv.AddCssClass("text-array-container form-input");
        itemsDiv.Attributes.Add("id", Id);

        var childContext = new TagHelperContext(
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: Guid.NewGuid().ToString("N")
        );

        if (Current != null)
        {

            foreach (var item in Current)
            {
                var childOutput = await InputTagHelperOutput(Name, item, childContext);
                itemsDiv.InnerHtml.AppendHtml(childOutput);
            }
        }
        // add the items div to the output
        titleDiv.InnerHtml.AppendHtml(itemsDiv);
        output.Content.AppendHtml(titleDiv);

        if (AllowAdd)
        {

            var modalId = Id + "Modal";
            var templateId = Id + "Template";

            var buttonHolderOutput = await FormRowContainerTagHelperOutput(
                $@"<button type=""button"" class=""success-button btn-small"" data-ck-modal-target=""{modalId}"">Add {Title}</button>"
                , childContext);
            output.Content.AppendHtml(buttonHolderOutput);

            
            output.Content.AppendHtml($@"<template id=""{templateId}"">");


            var childOutput = await InputTagHelperOutput(Name, "", childContext);
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

    private async Task<TagHelperOutput> InputTagHelperOutput(string name,  string item,  TagHelperContext childContext)
    {
        var selectionItemTagHelper = new InputItemTagHelper()
        {
            Name = name,
            Value = item
        };
                
        var childOutput = new TagHelperOutput(
            "input-item", 
            attributes: new TagHelperAttributeList
            {
                new TagHelperAttribute("name", Name),
                new TagHelperAttribute("value", item),
            },
            getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent())
        );
                
        await selectionItemTagHelper.ProcessAsync(childContext, childOutput);
        return childOutput;
    }

    private async Task<TagHelperOutput> FormRowContainerTagHelperOutput(string childContentString,  TagHelperContext childContext)
    {
        var selectionItemTagHelper = new FormRowContainerTagHelper(this.Generator);
                
        var childContent = new DefaultTagHelperContent();
        childContent.SetHtmlContent(childContentString);

        var childOutput = new TagHelperOutput(
            "form-row-container", 
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(childContent)
        );
                
        await selectionItemTagHelper.ProcessAsync(childContext, childOutput);
        return childOutput;
    }

    
    
}