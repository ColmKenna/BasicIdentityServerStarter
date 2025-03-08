using System.Linq.Expressions;
using IdentityServerAspNetIdentity.TagHelpers.FormRow;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using RazorExtensions;

namespace IdentityServerAspNetIdentity.UnitTests;

public class FormRowTextInputTagHelperTests
{
    private readonly Mock<IHtmlGenerator> _htmlGeneratorMock;
    private readonly ViewContext _viewContext;

    public FormRowTextInputTagHelperTests()
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

        _htmlGeneratorMock.Setup(x => x.GenerateTextBox(
                _viewContext,
                modelExpression.ModelExplorer,
                modelExpression.Name,
                modelExpression.ModelExplorer.Model,
                null,
                It.Is<Dictionary<string, object>>(attrs =>
                    attrs.ContainsKey("class") && attrs["class"].ToString() == inputClass)))
            .Returns(new TagBuilder("input").WithCssClass(inputClass));
    }

    /// <summary>
    /// Test model to generate ModelExpression dynamically
    /// </summary>
    public class TestModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsHidden { get; set; }
    }

    public class NestedTestModel
    {
        public TestModel Child { get; set; } = new TestModel();
    }

    /// <summary>
    /// Helper method to create ModelExpression for properties dynamically
    /// </summary>
    private static ModelExpression CreateModelExpression<TModel, TProperty>(TModel model,
        Expression<Func<TModel, TProperty>> expression)
    {
        var metadataProvider = new EmptyModelMetadataProvider();
        var modelMetadata = metadataProvider.GetMetadataForType(typeof(TModel));
        var modelExplorer = new ModelExplorer(metadataProvider, modelMetadata, model);

        var modelExpressionProvider = new ModelExpressionProvider(metadataProvider);
        return modelExpressionProvider.CreateModelExpression(
            new ViewDataDictionary<TModel>(metadataProvider, new ModelStateDictionary()) { Model = model }, expression);
    }

    /// <summary>
    /// Helper method to create and configure the tag helper instance
    /// </summary>
    private FormRowTextInputTagHelper CreateTagHelper(ModelExpression modelExpression, ModelExpression baseType = null)
    {
        return new FormRowTextInputTagHelper(_htmlGeneratorMock.Object)
        {
            For = modelExpression,
            ViewContext = _viewContext
        };
    }

    #region Repeated Test Setup Helpers

    /// <summary>
    /// Creates a TagHelperContext with default values.
    /// </summary>
    private TagHelperContext CreateTagHelperContext(string tagName = "form-row-text-input", string uniqueId = "test")
    {
        return new TagHelperContext(
            tagName,
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            uniqueId);
    }

    /// <summary>
    /// Creates a TagHelperOutput with default values.
    /// </summary>
    private TagHelperOutput CreateTagHelperOutput(string tagName = "form-row-text-input")
    {
        return new TagHelperOutput(
            tagName,
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
    }

    /// <summary>
    /// Creates a TestModel instance with Name set to "Test", generates the corresponding ModelExpression,
    /// and sets up the html generator mock.
    /// </summary>
    private ModelExpression CreateAndSetupTestModelExpression(string name = "Test")
    {
        var model = new TestModel { Name = name };
        var modelExpression = CreateModelExpression(model, m => m.Name);
        SetupHtmlGeneratorMock(modelExpression, "form-label", "form-input");
        return modelExpression;
    }

    #endregion

    [Fact]
    public async Task WhenIdSet_DivHasId()
    {
        // Arrange
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
        tagHelper.Id = "custom_id"; // Explicit ID set

        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("custom_id_Container", output.Attributes["id"].Value.ToString());
    }

    [Fact]
    public async Task WhenIdNotSet_DivHasGeneratedIdFromModel()
    {
        // Arrange
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
        tagHelper.Id = null;

        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("Name_Container", output.Attributes["id"].Value.ToString());
    }
    
    [Fact]
    public async Task DivHasFormRowClass()
    {
        // Arrange
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
    
        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Contains("form-row", output.Attributes["class"].Value.ToString());
    }
    
    [Fact]
    public async Task DivCombinesFormRowClassWithCustomClass()
    {
        // Arrange
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
        tagHelper.CssClass = "custom-class";

        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("form-row custom-class", output.Attributes["class"].Value.ToString());
    }
    
    [Fact]
    public async Task CallsGeneratesLabel()
    {
        // Arrange
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
    
        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        _htmlGeneratorMock.Verify(x => x.GenerateLabel(
                _viewContext,
                modelExpression.ModelExplorer,
                modelExpression.Name,
                null,
                It.Is<Dictionary<string, object>>(attrs => 
                    attrs["class"].ToString() == "form-label")),
            Times.Once);
    }
    
    [Fact]
    public async Task CallsGeneratesTextBox()
    {
        // Arrange
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
    
        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        _htmlGeneratorMock.Verify(x => x.GenerateTextBox(
                _viewContext,
                modelExpression.ModelExplorer,
                modelExpression.Name,
                modelExpression.ModelExplorer.Model,
                null,
                It.Is<Dictionary<string, object>>(attrs => 
                    attrs["class"].ToString() == "form-input")),
            Times.Once);
    }
    
    [Fact]
    public async Task CallsGeneratesTextBoxWithInputCssClass()
    {
        // Arrange
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
        tagHelper.InputCssClass = "custom-input";
    
        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        _htmlGeneratorMock.Verify(x => x.GenerateTextBox(
                _viewContext,
                modelExpression.ModelExplorer,
                modelExpression.Name,
                modelExpression.ModelExplorer.Model,
                null,
                It.Is<Dictionary<string, object>>(attrs => 
                    attrs["class"].ToString() == "form-input custom-input")),
            Times.Once);
    }
    
    [Fact]
    public async Task CallsGeneratesTextBoxWithCustomAttributes()
    {
        // Arrange
        var modelExpression = CreateAndSetupTestModelExpression();
        var tagHelper = CreateTagHelper(modelExpression);
        tagHelper.InputAttributes["data-custom"] = "value";
        tagHelper.InputAttributes["maxlength"] = "10";
    
        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        _htmlGeneratorMock.Verify(x => x.GenerateTextBox(
                _viewContext,
                modelExpression.ModelExplorer,
                modelExpression.Name,
                modelExpression.ModelExplorer.Model,
                null,
                It.Is<Dictionary<string, object>>(attrs => 
                    attrs["data-custom"].ToString() == "value" &&
                    attrs["maxlength"].ToString() == "10" &&
                    attrs["class"].ToString() == "form-input")),
            Times.Once);
    }
 
    [Fact]
    public async Task ForIsNull_ThrowsException()
    {
        // Arrange
        var tagHelper = new FormRowTextInputTagHelper(_htmlGeneratorMock.Object)
        {
            ViewContext = _viewContext
            // For is not set
        };

        var context = new TagHelperContext(
            tagName: "form-row-text-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "form-row-text-input",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => tagHelper.ProcessAsync(context, output));
    }

    [Fact]
    public async Task Hidden_True_RendersHiddenInput()
    {
        // Arrange
        var model = new TestModel { IsHidden = true };
        var modelExpression = CreateModelExpression(model, m => m.IsHidden);
        SetupHtmlGeneratorMock(modelExpression, "", "");

        var tagHelper = CreateTagHelper(modelExpression);
        tagHelper.Hidden = true;

        var context = new TagHelperContext(
            tagName: "form-row-text-input",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        var output = new TagHelperOutput(
            "form-row-text-input",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Null(output.TagName);
        Assert.Contains("hidden", output.Content.GetContent());
        Assert.DoesNotContain("div", output.Content.GetContent());
    }

    [Fact]
    public async Task WithBaseType_GeneratesCorrectName()
    {
        // Arrange
        var model = new NestedTestModel { Child = new TestModel { Name = "Child" } };
        var baseModelExpression = CreateModelExpression(model, m => m.Child);
        var modelExpression = CreateModelExpression(model.Child, m => m.Name);

        SetupHtmlGeneratorMock(modelExpression, "form-label", "form-input");

        var tagHelper = CreateTagHelper(modelExpression, baseModelExpression);
        tagHelper.Id = "test-id";
        tagHelper.BaseType = baseModelExpression;

        var context = CreateTagHelperContext("form-template-row-text-input");
        var output = CreateTagHelperOutput("form-template-row-text-input");

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        // The name should follow BaseType.Name[index].For.Name pattern
        _htmlGeneratorMock.Verify(x => x.GenerateTextBox(
                _viewContext,
                It.IsAny<ModelExplorer>(),
                "Child[replace_with_index].Name",
                "Child",
                null,
                It.IsAny<Dictionary<string, object>>()),
            Times.Once);
        
    }
    
    
    [Fact]
    public async Task WhenForNameHasDots_GeneratesCorrectContainerId()
    {
        // Arrange
        var modelExpression = CreateAndSetupTestModelExpression();
        // Create a new ModelExpression with a dot in the name.
        var modelExpressionWithDot = new ModelExpression("Address.Street", modelExpression.ModelExplorer);
        SetupHtmlGeneratorMock(modelExpressionWithDot, "form-label", "form-input");
    
        var tagHelper = CreateTagHelper(modelExpressionWithDot);
        tagHelper.Id = null; // Ensure it will be generated from For.Name

        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();
    
        // Act
        await tagHelper.ProcessAsync(context, output);
    
        // Assert: The container id should replace the dot with an underscore.
        Assert.Equal("Address_Street_Container", output.Attributes["id"].Value.ToString());
    }
}
