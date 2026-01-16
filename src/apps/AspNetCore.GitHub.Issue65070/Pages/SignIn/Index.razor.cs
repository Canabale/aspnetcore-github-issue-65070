using Microsoft.AspNetCore.Components;

namespace AspNetCore.GitHub.Issue65070.Pages.SignIn;

/// <summary>Represents the page that triggers authentication challenges.</summary>
[Route(EndpointPaths.SignIn)]
public partial class Index : ComponentBase
{
    /// <summary>Gets or sets the email address.</summary>
    /// <value>The email address of the user.</value>
    [Parameter]
    [SupplyParameterFromQuery]
    public string? LoginHint { get; set; }

    /// <summary>Gets or sets the URL to return to, once the user is authenticated.</summary>
    /// <value>The path of the endpoint to return to, once the user is authenticated.</value>
    [Parameter]
    [SupplyParameterFromQuery]
    public string? RedirectUri { get; set; }
}