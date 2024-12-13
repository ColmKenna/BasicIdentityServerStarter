using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.Tabs;

[HtmlTargetElement("tab-item", ParentTag = "tab")]
public class TabItemTagHelper : TagHelper
{
    public string Header { get; set; }
    public string TabId { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(TabId))
        {
            TabId = Header.Replace(" ", string.Empty).ToLower();
        }
        // Generate header and content
        var headerHtml = $@"
            <button type=""button"" class='tab-link' data-ck-tab-target='{TabId}'>{Header}</button>";
        var contentHtml = $@"
            <div id='{TabId}' class='tab'>
                <div class='tab-content'>
                    {output.GetChildContentAsync().Result.GetContent()}
                </div>
            </div>";

        // Append header and content to the parent's context
        if (context.Items.TryGetValue("TabHeaders", out var headersObj) && headersObj is List<string> headers)
        {
            headers.Add(headerHtml);
        }

        if (context.Items.TryGetValue("TabContents", out var contentsObj) && contentsObj is List<string> contents)
        {
            contents.Add(contentHtml);
        }

        // Suppress output as it will be managed by the parent
        output.SuppressOutput();
    }
}