using System.Linq.Expressions;
using IdentityServerAspNetIdentity.TagHelpers.FormRow;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using RazorExtensions;

namespace IdentityServerAspNetIdentity.UnitTests;

public class FormRowCheckboxTagHelperTests
{
    private readonly Mock<IHtmlGenerator> _htmlGeneratorMock;
    private readonly ViewContext _viewContext;

    public FormRowCheckboxTagHelperTests()
    {
        _htmlGeneratorMock = new Mock<IHtmlGenerator>();
        _viewContext = new ViewContext();
    }

    private void SetupHtmlGeneratorMock(ModelExpression modelExpression, string labelClass, string inputClass)
    {
        var labelHtmlAttributes = new Dictionary<string, object>
        {
            { "class", labelClass }
        };

        _htmlGeneratorMock.Setup(x => x.GenerateLabel(
                It.IsAny<ViewContext>(),
                It.IsAny<ModelExplorer>(),
                It.IsAny<string>(),
                It.IsAny<string>(), // allows null or any string
                It.Is<Dictionary<string, object>>(attrs =>
                    attrs.ContainsKey("class") && 
                    attrs["class"].ToString() == labelClass)))
            .Returns(new TagBuilder("label").WithCssClass(labelClass));

        _htmlGeneratorMock.Setup(x => x.GenerateCheckBox(
                _viewContext,
                modelExpression.ModelExplorer,
                modelExpression.Name,
                (bool?)modelExpression.Model ?? false,
                It.Is<Dictionary<string, object>>(attrs =>
                    attrs.ContainsKey("class") && attrs["class"].ToString() == inputClass)))
            .Returns(new TagBuilder("input").WithCssClass(inputClass));
    }

    public class TestModel
    {
        public bool IsChecked { get; set; }
    }

    private static ModelExpression CreateModelExpression<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression)
    {
        var metadataProvider = new EmptyModelMetadataProvider();
        var modelMetadata = metadataProvider.GetMetadataForType(typeof(TModel));
        var modelExplorer = new ModelExplorer(metadataProvider, modelMetadata, model);
        var modelExpressionProvider = new ModelExpressionProvider(metadataProvider);
        return modelExpressionProvider.CreateModelExpression(
            new ViewDataDictionary<TModel>(metadataProvider, new ModelStateDictionary()) { Model = model }, expression);
    }

    private FormRowCheckboxTagHelper CreateTagHelper(ModelExpression modelExpression)
    {
        return new FormRowCheckboxTagHelper(_htmlGeneratorMock.Object)
        {
            For = modelExpression,
            ViewContext = _viewContext
        };
    }

    private TagHelperContext CreateTagHelperContext() => new(
        tagName: "form-row-checkbox",
        allAttributes: new TagHelperAttributeList(),
        items: new Dictionary<object, object>(),
        uniqueId: "test");

    private TagHelperOutput CreateTagHelperOutput() => new(
        tagName: "form-row-checkbox",
        attributes: new TagHelperAttributeList(),
        getChildContentAsync: (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

    private ModelExpression CreateAndSetupTestModelExpression(bool isChecked = false, string additionalLabelClass = "", string additionalInputClass = "")
    {
        var model = new TestModel { IsChecked = isChecked };
        var modelExpression = CreateModelExpression(model, m => m.IsChecked);
        var labelClass = $"form-label {additionalLabelClass}".Trim();
        var inputClass = $"form-check-input {additionalInputClass}".Trim();
        SetupHtmlGeneratorMock(modelExpression, labelClass, inputClass); 
        return modelExpression;
    }
    [Fact]
    public async Task WhenIdSet_DivHasId()
    {
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
        tagHelper.Id = "custom_id";

        var output = CreateTagHelperOutput();
        await tagHelper.ProcessAsync(CreateTagHelperContext(), output);

        Assert.Equal("custom_id_Container", output.Attributes["id"].Value.ToString());
    }
    
    [Fact]
    public async Task WhenIdNotSet_DivHasGeneratedIdFromModel()
    {
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);

        var output = CreateTagHelperOutput();
        await tagHelper.ProcessAsync(CreateTagHelperContext(), output);

        Assert.Equal("IsChecked_Container", output.Attributes["id"].Value.ToString());
    }
    
    [Fact]
    public async Task DivHasFormRowClass()
    {
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);

        var output = CreateTagHelperOutput();
        await tagHelper.ProcessAsync(CreateTagHelperContext(), output);

        Assert.Contains("form-row", output.Attributes["class"].Value.ToString());
    }
    
    [Fact]
    public async Task DivCombinesFormRowClassWithCustomClass()
    {
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
        tagHelper.CssClass = "custom-class";

        var output = CreateTagHelperOutput();
        await tagHelper.ProcessAsync(CreateTagHelperContext(), output);

        Assert.Equal("form-row custom-class", output.Attributes["class"].Value.ToString());
    }
    
    [Fact]
    public async Task CallsGenerateLabel()
    {
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);

        await tagHelper.ProcessAsync(CreateTagHelperContext(), CreateTagHelperOutput());

        _htmlGeneratorMock.Verify(x => x.GenerateLabel(
                _viewContext,
                modelExpression.ModelExplorer,
                modelExpression.Name,
                null,
                It.Is<Dictionary<string, object>>(attrs => attrs["class"].ToString() == "form-label")),
            Times.Once);
    }
    
    [Fact]
    public async Task CallsGenerateCheckBox()
    {
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);

        await tagHelper.ProcessAsync(CreateTagHelperContext(), CreateTagHelperOutput());

        _htmlGeneratorMock.Verify(x => x.GenerateCheckBox(
                _viewContext,
                modelExpression.ModelExplorer,
                modelExpression.Name,
                false,
                It.Is<Dictionary<string, object>>(attrs => attrs["class"].ToString() == "form-check-input")),
            Times.Once);
    }
    
        [Fact]
    public async Task CallsGenerateCheckBoxWithInputCssClass()
    {
        var modelExpression = CreateAndSetupTestModelExpression(false,"", "custom-input");
        var tagHelper = CreateTagHelper(modelExpression);
        tagHelper.InputCssClass = "custom-input";
        
      
        await tagHelper.ProcessAsync(CreateTagHelperContext(), CreateTagHelperOutput());

        _htmlGeneratorMock.Verify(x => x.GenerateCheckBox(
            _viewContext,
            modelExpression.ModelExplorer,
            modelExpression.Name,
            false,
            It.Is<Dictionary<string, object>>(attrs => attrs["class"].ToString() == "form-check-input custom-input")),
        Times.Once);
    }

    [Fact]
    public async Task CallsGenerateCheckBoxWithCustomAttributes()
    {
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
        tagHelper.InputAttributes["data-custom"] = "value";
        tagHelper.InputAttributes["disabled"] = "disabled";

        await tagHelper.ProcessAsync(CreateTagHelperContext(), CreateTagHelperOutput());

        _htmlGeneratorMock.Verify(x => x.GenerateCheckBox(
            _viewContext,
            modelExpression.ModelExplorer,
            modelExpression.Name,
            false,
            It.Is<Dictionary<string, object>>(attrs => 
                attrs["data-custom"].ToString() == "value" &&
                attrs["disabled"].ToString() == "disabled")),
        Times.Once);
    }

    [Fact]
    public async Task SliderTrue_RendersSliderElements()
    {
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
        tagHelper.Slider = true;

        var output = CreateTagHelperOutput();
        await tagHelper.ProcessAsync(CreateTagHelperContext(), output);

        var content = output.Content.GetContent();
        Assert.Contains("form-switch", content);
        Assert.Contains("<span class=\"form-slider\"></span>", content);
    }
    
    [Fact]
    public async Task CallToGenerateCheckBoxValueIsTrueWhenChecked()
    {
        var modelExpression = CreateAndSetupTestModelExpression(true);
        var tagHelper = CreateTagHelper(modelExpression);
        tagHelper.Slider = true;

        var output = CreateTagHelperOutput();
        await tagHelper.ProcessAsync(CreateTagHelperContext(), output);

        var content = output.Content.GetContent();
        
        _htmlGeneratorMock.Verify(x => x.GenerateCheckBox(
                _viewContext,
                modelExpression.ModelExplorer,
                modelExpression.Name,
                true,
                It.IsAny<Dictionary<string, object>>()),
            Times.Once);

        
    }
    
    [Fact]
    public async Task WhenLabelCssClassIsSet_LabelHasFormLabelAndCustomClass()
    {
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
        tagHelper.LabelCssClass = "custom-label";

        var output = CreateTagHelperOutput();
        await tagHelper.ProcessAsync(CreateTagHelperContext(), output);

        _htmlGeneratorMock.Verify(x => x.GenerateLabel(
                _viewContext,
                modelExpression.ModelExplorer,
                modelExpression.Name,
                null,
                It.Is<Dictionary<string, object>>(attrs =>
                    attrs["class"].ToString() == "form-label custom-label")),
            Times.Once);
    }
    
    [Fact]
    public async Task WhenSliderIsFalse_RendersFormCheckDivAndNoFormSliderSpan()
    {
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
        // Slider = false by default

        var output = CreateTagHelperOutput();
        await tagHelper.ProcessAsync(CreateTagHelperContext(), output);

        var content = output.Content.GetContent();
        Assert.Contains("form-check", content);
        Assert.DoesNotContain("form-switch", content);
        Assert.DoesNotContain("form-slider", content);
    }

}
