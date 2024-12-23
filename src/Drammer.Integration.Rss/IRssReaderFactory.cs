namespace Drammer.Integration.Rss;

/// <summary>
/// The RSS reader factory interface.
/// </summary>
public interface IRssReaderFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IRssReader"/>.
    /// </summary>
    /// <param name="rssFeedUrl">The RSS feed URL.</param>
    /// <returns>An <see cref="IRssReader"/>.</returns>
    IRssReader Create(string rssFeedUrl);
}