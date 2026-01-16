using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.GitHub.Issue65070.Controllers;

/// <summary>Provides endpoints for handling user authentication operations within the application.</summary>
public class AuthenticationController : Controller
{
    /// <summary>Challenges the Microsoft authentication scheme.</summary>
    /// <param name="loginHint">The login hint. This would typically be the email address to authenticate.</param>
    /// <param name="redirectUri">The URL to redirect to after authentication.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [ValidateAntiForgeryToken]
    [Route(EndpointPaths.SignInChallenge)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> ChallengeAsync(string? loginHint = null, string? redirectUri = null) => this.Challenge(
        new MicrosoftChallengeProperties()
        {
            RedirectUri = string.IsNullOrEmpty(redirectUri) ? "/" : redirectUri,
            LoginHint = loginHint,
        },
        MicrosoftAccountDefaults.AuthenticationScheme);

    /// <summary>Signs out the current user and redirects to the specified return URL or the home page.</summary>
    /// <param name="redirectUri">
    /// The URL to redirect to after sign-out.
    /// If <see langword="null"/> or empty, redirects to the root ("/").
    /// </param>
    /// <returns>An <see cref="IActionResult"/> that redirects the user after sign-out.</returns>
    [Route(EndpointPaths.SignOut)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> SignOutAsync(string? redirectUri = null)
    {
        await this.HttpContext.SignOutAsync();
        var location = string.IsNullOrEmpty(redirectUri) ? "/" : redirectUri;
        return this.Redirect(location);
    }
}
