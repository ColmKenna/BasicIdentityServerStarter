﻿using IdentityServerAspNetIdentity.TagHelpers.Tabs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.UnitTests;

public class TabTagHelperTests
{
    // Helper method to create TagHelperContext
    private TagHelperContext CreateTagHelperContext()
    {
        return new TagHelperContext(
            tagName: "tab",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object>(),
            uniqueId: "test-parent");
    }

    // Helper method to create TagHelperOutput with specific child content
    private TagHelperOutput CreateTagHelperOutputWithContent(string childContent)
    {
        var output = new TagHelperOutput(
            "tab",
            new TagHelperAttributeList(),
            (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetHtmlContent(childContent); // Use provided child content
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
        // Pre-set TagName and Class attribute as the TagHelper would do before processing content
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "tabs");
        return output;
    }



    [Fact]
    public async Task ProcessAsync_SelectsFirstTabItem_WhenNoneAreSelected()
    {
        // Arrange
        var tagHelper = new TabTagHelper();
        var context = CreateTagHelperContext();
        // Simulate child content generated by TabItemTagHelper (none selected)
        var childHtml = "<input class=\"tabs-panel-input\" name=\"tabs\" type=\"radio\" id=\"tab1\" />" +
                        "<label class=\"tab-heading\" for=\"tab1\">Tab 1</label>" +
                        "<div class=\"panel\"><div class=\"panel-content\">Content 1</div></div>" +
                        "<input class=\"tabs-panel-input\" name=\"tabs\" type=\"radio\" id=\"tab2\" />" +
                        "<label class=\"tab-heading\" for=\"tab2\">Tab 2</label>" +
                        "<div class=\"panel\"><div class=\"panel-content\">Content 2</div></div>";
        var output = CreateTagHelperOutputWithContent(childHtml);


        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var expectedHtml = "<input class=\"tabs-panel-input\" name=\"tabs\" type=\"radio\" id=\"tab1\" checked=\"checked\"/>" + // First one should be checked
                           "<label class=\"tab-heading\" for=\"tab1\">Tab 1</label>" +
                           "<div class=\"panel\"><div class=\"panel-content\">Content 1</div></div>" +
                           "<input class=\"tabs-panel-input\" name=\"tabs\" type=\"radio\" id=\"tab2\" />" +
                           "<label class=\"tab-heading\" for=\"tab2\">Tab 2</label>" +
                           "<div class=\"panel\"><div class=\"panel-content\">Content 2</div></div>";
        Assert.Equal("div", output.TagName);
        Assert.Equal("tabs", output.Attributes["class"].Value);
        Assert.Equal(expectedHtml, output.Content.GetContent());
    }

    [Fact]
    public async Task ProcessAsync_DoesNotChangeSelection_WhenATabIsAlreadySelected()
    {
        // Arrange
        var tagHelper = new TabTagHelper();
        var context = CreateTagHelperContext();
        // Simulate child content with the second tab selected
        var childHtml = "<input class=\"tabs-panel-input\" name=\"tabs\" type=\"radio\" id=\"tab1\" />" +
                        "<label class=\"tab-heading\" for=\"tab1\">Tab 1</label>" +
                        "<div class=\"panel\"><div class=\"panel-content\">Content 1</div></div>" +
                        "<input class=\"tabs-panel-input\" name=\"tabs\" type=\"radio\" id=\"tab2\" checked=\"checked\"/>" + // Second one is checked
                        "<label class=\"tab-heading\" for=\"tab2\">Tab 2</label>" +
                        "<div class=\"panel\"><div class=\"panel-content\">Content 2</div></div>";
        var output = CreateTagHelperOutputWithContent(childHtml);

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        // Expected HTML is the same as the input childHtml because selection shouldn't change
        Assert.Equal("div", output.TagName);
        Assert.Equal("tabs", output.Attributes["class"].Value);
        Assert.Equal(childHtml, output.Content.GetContent());
    }

    [Fact]
    public async Task ProcessAsync_HandlesEmptyChildContentGracefully()
    {
        // Arrange
        var tagHelper = new TabTagHelper();
        var context = CreateTagHelperContext();
        var childHtml = ""; // No child content
        var output = CreateTagHelperOutputWithContent(childHtml);

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("div", output.TagName);
        Assert.Equal("tabs", output.Attributes["class"].Value);
        Assert.Equal("", output.Content.GetContent()); // Content remains empty
    }

    [Fact]
    public async Task ProcessAsync_HandlesChildContentWithoutInputsGracefully()
    {
        // Arrange
        var tagHelper = new TabTagHelper();
        var context = CreateTagHelperContext();
        var childHtml = "<p>Some other content</p>"; // No tab inputs
        var output = CreateTagHelperOutputWithContent(childHtml);

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("div", output.TagName);
        Assert.Equal("tabs", output.Attributes["class"].Value);
        Assert.Equal("<p>Some other content</p>", output.Content.GetContent()); // Content remains unchanged
    }
}