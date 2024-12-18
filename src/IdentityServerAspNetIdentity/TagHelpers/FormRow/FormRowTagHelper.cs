#nullable enable
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.FormRow;

[HtmlTargetElement("form-row", Attributes = "for")]
public class FormRowTagHelper : TagHelper
{
    private const string BooleanType = "System.Boolean";


    [HtmlAttributeName("for")] public ModelExpression For { get; set; } = null!;

    public string Label { get; set; } = string.Empty;

    [HtmlAttributeName("source")] public IEnumerable<string>? Source { get; set; } = null!;

    [HtmlAttributeName("use-radio-buttons")]
    public bool UseRadioButtons { get; set; } = false;

    [HtmlAttributeName("use-checkboxes")] public bool UseCheckboxes { get; set; } = false;


    [HtmlAttributeNotBound] [ViewContext] public ViewContext ViewContext { get; set; }

    /// <summary>
    /// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="LabelTagHelper"/>'s output.
    /// </summary>
    protected IHtmlGenerator Generator { get; }

    public bool Hidden { get; set; } = false;


    public FormRowTagHelper(IHtmlGenerator generator)
    {
        Generator = generator;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!Hidden)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "form-row");

            TagBuilder? labelTagBuilder = Generator.GenerateLabel(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                labelText: Label == String.Empty ? null : Label,
                htmlAttributes: new Dictionary<string, object>
                {
                    {"class", "form-label"}
                }
            );

            output.Content.AppendHtml(labelTagBuilder);
        }
        else
        {
            output.TagName = "";
        }


        // var labelTag = $"<label asp-for=\"{For.Name}\" class=\"form-label\">{LabelText}</label>";

        string inputTag;
        if (Source != null)
        {
            if (UseRadioButtons)
            {
                inputTag = "<fieldset class=\"form-group\">";
                inputTag += $"<legend>{Label}</legend>";


                foreach (var item in Source)
                {
                    inputTag += $"<div class=\"form-check\">" +
                                $"<input type=\"radio\" name=\"{For.Name}\" class=\"form-check-input\" value=\"{item}\" />" +
                                $"<label class=\"form-check-label\" for=\"{For.Name}_{item}\">{item}</label>" +
                                "</div>";
                }

                inputTag += "</fieldset>";
            }
            else if (UseCheckboxes)
            {
                inputTag = "<fieldset class=\"form-group\">";
                inputTag += $"<legend>{Label}</legend>";
                foreach (var item in Source)
                {
                    inputTag += $"<div class=\"form-check\">" +
                                $"<input type=\"checkbox\" name=\"{For.Name}\" class=\"form-check-input\" value=\"{item}\" />" +
                                $"<label class=\"form-check-label\" for=\"{For.Name}_{item}\">{item}</label>" +
                                "</div>";
                }

                inputTag += "</fieldset>";
            }
            else
            {
                inputTag = "<select asp-for=\"{For.Name}\" class=\"form-input\">";
                foreach (var item in Source)
                {
                    inputTag += $"<option value=\"{item}\">{item}</option>";
                }

                inputTag += "</select>";
            }
        }
        else if (For.Metadata.ModelType == typeof(bool) || For.Metadata.ModelType == typeof(bool?))
        {
            var checkedAttribute = For.Model != null && (bool) For.Model ? "checked" : "";
            var checkLabelTagHelper = Generator.GenerateCheckBox(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                isChecked: false,
                htmlAttributes: new Dictionary<string, object>
                {
                    {"class", "form-check"},
                    {"type", Hidden ? "hidden" : "checkbox"},
                    {"checked", checkedAttribute}
                }
            );

            var wrapperDiv = new TagBuilder("div");
            wrapperDiv.AddCssClass("form-input");
            wrapperDiv.InnerHtml.AppendHtml(checkLabelTagHelper);

            output.Content.AppendHtml(wrapperDiv);
        }
        else
        {
            // text input
            var textAreaTagHelper = Generator.GenerateTextBox(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                value: For.Model,
                format: null,
                htmlAttributes: new Dictionary<string, object>
                {
                    {"class", "form-input"},
                    {"type", Hidden ? "hidden" : "text"}
                }
            );

            output.Content.AppendHtml(textAreaTagHelper);
        }


        var validationTagHelper = Generator.GenerateValidationMessage(
            ViewContext,
            For.ModelExplorer,
            For.Name,
            message: null,
            tag: "span",
            htmlAttributes: new Dictionary<string, object>
            {
                {"class", "text-danger"}
            }
        );

        output.Content.AppendHtml(validationTagHelper);
    }
}