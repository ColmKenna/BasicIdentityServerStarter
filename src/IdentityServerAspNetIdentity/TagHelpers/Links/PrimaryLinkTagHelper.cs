using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityServerAspNetIdentity.TagHelpers.Links;

[HtmlTargetElement("primary-link")]
public class PrimaryLinkTagHelper : BaseLinkTagHelper
{
    protected override string ButtonClass => "primary-button";
}