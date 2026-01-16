using Microsoft.AspNetCore.Mvc.ApplicationParts;
using AspNetCore.GitHub.Issue65070.Pages.Shared;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace AspNetCore.GitHub.Issue65070;

/// <summary>Represents the root component of the application.</summary>
public partial class App : ComponentBase
{
    private static IReadOnlyCollection<Assembly>? _additionalAssemblies;

    [Inject]
    private ApplicationPartManager ApplicationPartManager { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _additionalAssemblies ??= this.ApplicationPartManager.ApplicationParts
            .OfType<AssemblyPart>()
            .Select(part => part.Assembly)
            .ToArray();
    }

    /// <summary>Gets the layout of the specified page type.</summary>
    /// <param name="pageType">The page type to get the layout for.</param>
    /// <returns>The type of the layout.</returns>
    private static Type GetLayout(Type pageType)
        => pageType.GetCustomAttributes<LayoutAttribute>(true).FirstOrDefault()?.LayoutType ?? typeof(MainLayout);
}