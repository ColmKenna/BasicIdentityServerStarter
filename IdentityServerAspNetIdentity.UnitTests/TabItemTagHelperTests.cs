using IdentityServerAspNetIdentity.TagHelpers.Tabs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.UnitTests;

public class TabItemTagHelperTests
{
    // Helper method to create TagHelperContext
    private TagHelperContext CreateTagHelperContext()
    {
        return new TagHelperContext(
            tagName: "tab-item",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test");
    }

    // Helper method to create TagHelperOutput
    private TagHelperOutput CreateTagHelperOutput(string tagName = "tab-item", TagHelperAttributeList attributes = null)
    {
        attributes ??= new TagHelperAttributeList();
        return new TagHelperOutput(
            tagName,
            attributes,
            (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent("Test Child Content"); // Default child content for testing
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
    }

    [Fact]
    public async Task ProcessAsync_GeneratesCorrectHtml_WithIdAndHeading()
    {
        // Arrange
        var tagHelper = new TabItemTagHelper
        {
            Id = "test-tab",
            Heading = "Test Heading"
        };
        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = "<input class=\"tabs-panel-input\" name=\"tabs\" type=\"radio\" id=\"test-tab\" />" +
                           "<label class=\"tab-heading\" for=\"test-tab\">Test Heading</label>" +
                           "<div class=\"panel\"><div class=\"panel-content\">Test Child Content</div></div>";
        Assert.Null(output.TagName); // Original tag should be removed
        Assert.Equal(expectedHtml, output.Content.GetContent());
    }

    [Fact]
    public async Task ProcessAsync_GeneratesCorrectHtml_WhenSelectedIsTrue()
    {
        // Arrange
        var tagHelper = new TabItemTagHelper
        {
            Id = "selected-tab",
            Heading = "Selected Tab",
            Selected = true
        };
        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = "<input class=\"tabs-panel-input\" name=\"tabs\" type=\"radio\" id=\"selected-tab\" checked=\"checked\"/>" +
                           "<label class=\"tab-heading\" for=\"selected-tab\">Selected Tab</label>" +
                           "<div class=\"panel\"><div class=\"panel-content\">Test Child Content</div></div>";
        Assert.Null(output.TagName);
        Assert.Equal(expectedHtml, output.Content.GetContent());
    }

    [Fact]
    public async Task ProcessAsync_GeneratesCorrectHtml_WhenSelectedIsFalse()
    {
        // Arrange
        var tagHelper = new TabItemTagHelper
        {
            Id = "unselected-tab",
            Heading = "Unselected Tab",
            Selected = false // Explicitly false (default)
        };
        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = "<input class=\"tabs-panel-input\" name=\"tabs\" type=\"radio\" id=\"unselected-tab\" />" +
                           "<label class=\"tab-heading\" for=\"unselected-tab\">Unselected Tab</label>" +
                           "<div class=\"panel\"><div class=\"panel-content\">Test Child Content</div></div>";
        Assert.Null(output.TagName);
        Assert.Equal(expectedHtml, output.Content.GetContent());
    }

    [Fact]
    public async Task ProcessAsync_GeneratesIdFromHeading_WhenIdIsNull()
    {
        // Arrange
        var tagHelper = new TabItemTagHelper
        {
            Heading = "Generate My ID" // Heading with spaces and mixed case
        };
        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedId = "generate-my-id"; // Generated ID
        var expectedHtml = $"<input class=\"tabs-panel-input\" name=\"tabs\" type=\"radio\" id=\"{expectedId}\" />" +
                           $"<label class=\"tab-heading\" for=\"{expectedId}\">Generate My ID</label>" +
                           "<div class=\"panel\"><div class=\"panel-content\">Test Child Content</div></div>";
        Assert.Null(output.TagName);
        Assert.Equal(expectedHtml, output.Content.GetContent());
        Assert.Equal(expectedId, tagHelper.Id); // Check if the Id property was updated
    }

    [Fact]
    public async Task ProcessAsync_GeneratesIdFromHeading_WithSpecialChars()
    {
        // Arrange
        var tagHelper = new TabItemTagHelper
        {
            Heading = "Tab #1 with $ymbols!" // Heading with special chars
        };
        var context = CreateTagHelperContext();
        var output = CreateTagHelperOutput();

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedId = "tab-1-with-ymbols"; // Generated ID, special chars removed, spaces to hyphens
        var expectedHtml = $"<input class=\"tabs-panel-input\" name=\"tabs\" type=\"radio\" id=\"{expectedId}\" />" +
                           $"<label class=\"tab-heading\" for=\"{expectedId}\">Tab #1 with $ymbols!</label>" + // Heading remains unchanged in label
                           "<div class=\"panel\"><div class=\"panel-content\">Test Child Content</div></div>";
        Assert.Null(output.TagName);
        Assert.Equal(expectedHtml, output.Content.GetContent());
        Assert.Equal(expectedId, tagHelper.Id);
    }

    [Fact]
    public async Task ProcessAsync_RendersChildContentCorrectly()
    {
        // Arrange
        var tagHelper = new TabItemTagHelper
        {
            Id = "content-tab",
            Heading = "Content Tab"
        };
        var context = CreateTagHelperContext();
        // Create output with specific child content
        var output = new TagHelperOutput(
            "tab-item",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetHtmlContent("<p>Specific Child HTML</p>"); // Set specific HTML
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });


        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = "<input class=\"tabs-panel-input\" name=\"tabs\" type=\"radio\" id=\"content-tab\" />" +
                           "<label class=\"tab-heading\" for=\"content-tab\">Content Tab</label>" +
                           "<div class=\"panel\"><div class=\"panel-content\"><p>Specific Child HTML</p></div></div>"; // Child content included
        Assert.Null(output.TagName);
        Assert.Equal(expectedHtml, output.Content.GetContent());
    }
}