namespace Drammer.Integration.Rss.Models;

/// <summary>
/// The category.
/// </summary>
public sealed record Category
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Category"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="label">The label.</param>
    /// <param name="scheme">The scheme.</param>
    public Category(string? name, string? label, string? scheme)
    {
        Name = name;
        Label = label;
        Scheme = scheme;
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Gets the label.
    /// </summary>
    public string? Label { get; }

    /// <summary>
    /// Gets the scheme.
    /// </summary>
    public string? Scheme { get; }
}