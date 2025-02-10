#nullable enable
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.FormRow;

[HtmlTargetElement("form-row-numeric-slider-input", Attributes = "asp-for" )]
[HtmlTargetElement("form-template-numeric-slider-input", Attributes = "asp-for,base-type" )]
public class FormRowNumericSliderInputTagHelper : TagHelper
{
    private readonly IHtmlGenerator _htmlGenerator;

    public FormRowNumericSliderInputTagHelper(IHtmlGenerator htmlGenerator)
    {
        _htmlGenerator = htmlGenerator;
    }

    [HtmlAttributeName("asp-for")] public ModelExpression For { get; set; }

    [HtmlAttributeName("base-type")] public ModelExpression? BaseType { get; set; }
    

    [HtmlAttributeNotBound] [ViewContext] public ViewContext ViewContext { get; set; }

    [HtmlAttributeName("class")] public string CssClass { get; set; }

    [HtmlAttributeName("label-class")] public string LabelCssClass { get; set; }

    [HtmlAttributeName("input-class")] public string InputCssClass { get; set; }

    // id
    [HtmlAttributeName("id")] public string Id { get; set; }

    [HtmlAttributeName("input-attributes", DictionaryAttributePrefix = "input-")]
    public IDictionary<string, object> InputAttributes { get; set; } = new Dictionary<string, object>();

    // min
    [HtmlAttributeName("min")] public int? Min { get; set; } 
    
    // max
    [HtmlAttributeName("max")] public int? Max { get; set; } 
    
    // step
    [HtmlAttributeName("step")] public int Step { get; set; } = 1;
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        
        var name = "";

        name = BaseType != null 
            ? $"{BaseType.Name}[replace_with_index].{For.Name}" 
            : For.Name;

        For = new ModelExpression(name, For.ModelExplorer);


        
        
        if (string.IsNullOrEmpty(Id))
        {
            Id = For.Name.Replace(".", "_");
        }

        var divId = Id?.Trim() + "_Container";

        output.Attributes.SetAttribute("id", divId);

        if (For == null) throw new InvalidOperationException("The 'asp-for' attribute is required.");


        // Set up the outer div
        output.TagName = "div";
        var divClass = $"form-row  {CssClass}".Trim();
        output.Attributes.SetAttribute("class", divClass);

        // Generate the label
        var labelClass = $"form-label {LabelCssClass}".Trim();
        var labelHtmlAttributes = new Dictionary<string, object>
        {
            { "class", labelClass }
        };


        var labelTag = _htmlGenerator.GenerateLabel(
            viewContext: ViewContext,
            modelExplorer: For.ModelExplorer,
            expression: For.Name,
            labelText: null, // Use the default label text from model metadata
            htmlAttributes: labelHtmlAttributes);

        output.Content.AppendHtml(labelTag);
        
        // if min is not set, set it to the for metadata min value
        
        
        var tag = new TagBuilder("div");
        var inputClass = $"form-input slider-container {InputCssClass}".Trim();
        tag.AddCssClass(inputClass);
        var sliderId = Id?.Trim() + "_Slider";
        // Generate the input element
        var inputAttributes = new Dictionary<string, object>(InputAttributes)
        {
            { "type", "number" },
            { "step", Step }
        };

        Min = Min ?? int.Parse(For.Metadata.ValidatorMetadata.OfType<RangeAttribute>().First().Minimum.ToString() ?? "0");
        Max = Max ?? int.Parse(For.Metadata.ValidatorMetadata.OfType<RangeAttribute>().First().Maximum.ToString() ?? For.Model.ToString());
        
        
        var inputTag = _htmlGenerator.GenerateTextBox(
            viewContext: ViewContext,
            modelExplorer: For.ModelExplorer,
            expression: For.Name,
            value: For.Model, // Let the generator handle the value
            format: null,
            htmlAttributes: inputAttributes);

        var inputTagId = inputTag.Attributes["id"];

        // Generate the slider
        var sliderClass = "form-slider";
        var sliderAttributes = new Dictionary<string, object?>
        {
            { "class", sliderClass },
            { "type", "range" },
            { "min", Min },
            { "max", Max },
            { "step", Step },
            { "id", sliderId },
            { "value", For.Model }
             
        };
        
        var sliderTag = new TagBuilder("input");
        sliderTag.MergeAttributes(sliderAttributes);

        tag.InnerHtml.AppendHtml(sliderTag);
        tag.InnerHtml.AppendHtml(inputTag);

        output.Content.AppendHtml(tag);

        
            
    }
}