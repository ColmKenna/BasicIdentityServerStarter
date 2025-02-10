using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RazorExtensions;

public static class HtmlContentExtensions
{
    /// <summary>
    /// Wraps the current HTML content in a div with the specified CSS class.
    /// </summary>
    /// <param name="content">inner content.</param>
    /// <param name="cssClass">The CSS class for the wrapper</param>
    /// <returns>A new TagBuilder with the div wrapping the content.</returns>
    public static TagBuilder CreateWrappedDiv(this IHtmlContent content, string cssClass)
     => content.CreateWrappedTag("div", cssClass);
    
    public static TagBuilder CreateWrappedTag(this IHtmlContent content, string tag, string cssClass)
    {
        ArgumentNullException.ThrowIfNull(content);
        var wrapper = new TagBuilder(tag);
        wrapper.AddCssClassIfNotNullOrWhiteSpace(cssClass);
        wrapper.InnerHtml.AppendHtml(content);
        return wrapper;
    }
    
    

    /// <summary>
    /// Adds the specified CSS class to the <see cref="TagBuilder"/> instance if the provided CSS class string is not null, empty, or composed only of white-space characters.
    /// </summary>
    /// <param name="content">The <see cref="TagBuilder"/> instance to which the CSS class will be added.</param>
    /// <param name="cssClass">The CSS class to add.</param>
    /// <returns>The updated <see cref="TagBuilder"/> instance.</returns>
    public static TagBuilder AddCssClassIfNotNullOrWhiteSpace(this TagBuilder content, string cssClass)
    {
        if (!string.IsNullOrWhiteSpace(cssClass))
        {
            content.AddCssClass(cssClass);
        }
        return content;
    }

}