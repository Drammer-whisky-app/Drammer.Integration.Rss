namespace Drammer.Integration.Rss.Models;

/// <summary>
/// The link.
/// </summary>
public sealed record Link
{
    /// <summary>
    /// Gets the url.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// Gets the title.
    /// </summary>
    public string? Title { get; init; }
}