using System.Xml.XPath;
using FunctionalExtensionMethods;
using HtmlAgilityPack;

namespace IdentityServerAspNetIdentity.UnitTests;

public static class HtmlDocumentExtensions
{
    public static HtmlNode GetHtmlElement(this HtmlDocument document, string id = null, string className = null, string name = null)
    {
        document.ThrowIfNull(nameof(document));

        return GetHtmlElement(document.DocumentNode, id, className, name);
    }

    public static HtmlNode GetHtmlElement(this HtmlNode node, string id = null, string className = null, string name = null)
    {
        node.ThrowIfNull(nameof(node));
        new[]{id, className,name}.ThrowIfAllAreNullOrEmpty("At least one of 'id', 'className', 'name' must be provided.");
        

        var xpathConditions = new List<string>();

        if (!string.IsNullOrEmpty(id))
            xpathConditions.Add($"(@id='{id}')");

        if (!string.IsNullOrEmpty(className))
            xpathConditions.Add($"(contains(concat(' ', normalize-space(@class), ' '), ' {className} '))");

        if (!string.IsNullOrEmpty(name))
            xpathConditions.Add($"(@name='{name}')");
        string xpath = "//*";
        xpath += $"[{string.Join(" and ", xpathConditions)}]";

        return node.SelectSingleNode(xpath);
    }   
    
    
    

    
}