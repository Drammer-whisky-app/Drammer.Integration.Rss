namespace Drammer.Integration.Rss.Models;

/// <summary>
/// The author.
/// </summary>
public sealed record Author
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Author"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="email">The email address.</param>
    /// <param name="uri">The uri.</param>
    public Author(string? name, string? email, string? uri)
    {
        Name = name;
        Email = email;
        Uri = uri;
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Gets the email.
    /// </summary>
    public string? Email { get; }

    /// <summary>
    /// Gets the uri.
    /// </summary>
    public string? Uri { get; }
}