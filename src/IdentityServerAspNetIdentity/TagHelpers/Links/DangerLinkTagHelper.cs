using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.Links;

[HtmlTargetElement("danger-link")]
public class DangerLinkTagHelper : BaseLinkTagHelper
{
    protected override string ButtonClass => "danger-button";
}