using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Primitives;

namespace IdentityServerAspNetIdentity.TagHelpers.Links;

public enum Size
{
    Small,
    Medium,
    Large
}

[HtmlTargetElement("info-link")]
public class InfoLinkTagHelper : BaseLinkTagHelper
{
    protected override string ButtonClass => "info-button";
}