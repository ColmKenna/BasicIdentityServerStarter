using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers;

public static class TagHelperOutputExtensions
{
    /// <summary>
    /// Appends a CSS class to the tag helper output's 'class' attribute.
    /// If the class attribute doesn't exist, it will create it.
    /// Otherwise, it will append the new class at the end.
    /// </summary>
    /// <param name="output">The TagHelperOutput to modify.</param>
    /// <param name="cssClass">The CSS class to append.</param>
    public static void AddClass(this TagHelperOutput output, string cssClass)
    {
        if (output == null) throw new ArgumentNullException(nameof(output));
        if (string.IsNullOrWhiteSpace(cssClass)) return;

        var existingClass = output.Attributes["class"]?.Value?.ToString();

        // If no class attribute exists, just set it
        if (string.IsNullOrEmpty(existingClass))
        {
            output.Attributes.SetAttribute("class", cssClass);
        }
        else
        {
            // Append the new class with a preceding space
            output.Attributes.SetAttribute("class", $"{existingClass} {cssClass}");
        }
    }
}