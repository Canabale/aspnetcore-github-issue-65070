namespace AspNetCore.GitHub.Issue65070;

/// <summary>Provides the paths of the endpoints of this app.</summary>
public static class EndpointPaths
{
    /// <summary>The path that the application redirects to, when an authentication process succeeded.</summary>
    public const string Authenticated = "/authenticated";

    /// <summary>The path of the sign-in endpoints.</summary>
    public const string SignIn = "/signin";

    /// <summary>The path of the sign-in challenge endpoint.</summary>
    public const string SignInChallenge = SignIn + "/challenge";

    /// <summary>The endpoint the signs out the user.</summary>
    public const string SignOut = "/signout";
}