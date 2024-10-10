using Drammer.Common.Extensions;

namespace Drammer.Integration.Rss.Models;

/// <summary>
/// The article.
/// </summary>
public sealed record Article
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Article"/> class.
    /// </summary>
    public Article(
        string? id,
        string? title,
        DateTime published,
        string? summary,
        string? url,
        string? copyright,
        IEnumerable<Author> authors,
        IEnumerable<Category> categories,
        IEnumerable<Link> links,
        IEnumerable<string> images)
    {
        Id = id;
        Title = title;
        Published = published;
        Summary = summary;
        Copyright = copyright;
        Authors = authors.ToList().AsReadOnly();
        Categories = categories.ToList().AsReadOnly();
        Links = links.ToList().AsReadOnly();
        Images = images.ToList().AsReadOnly();

        if (string.IsNullOrEmpty(url) && !Links.IsNullOrEmpty())
        {
            Url = Links.First().Url;
        }
        else
        {
            Url = url;
        }
    }

    /// <summary>
    /// Gets the id.
    /// </summary>
    public string? Id { get; }

    /// <summary>
    /// Gets the title.
    /// </summary>
    public string? Title { get; }

    /// <summary>
    /// Gets the published.
    /// </summary>
    public DateTime Published { get; }

    /// <summary>
    /// Gets the summary.
    /// </summary>
    public string? Summary { get;  }

    /// <summary>
    /// Gets the authors.
    /// </summary>
    public IReadOnlyCollection<Author> Authors { get; }

    /// <summary>
    /// Gets the url.
    /// </summary>
    public string? Url { get; }

    /// <summary>
    /// Gets the copyright.
    /// </summary>
    public string? Copyright { get; }

    /// <summary>
    /// Gets the categories.
    /// </summary>
    public IReadOnlyCollection<Category> Categories { get; }

    /// <summary>
    /// Gets the links.
    /// </summary>
    public IReadOnlyCollection<Link> Links { get; }

    /// <summary>
    /// Gets the images.
    /// </summary>
    public IReadOnlyCollection<string> Images { get; }
}