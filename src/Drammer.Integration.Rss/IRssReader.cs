using Drammer.Common;
using Drammer.Integration.Rss.Models;

namespace Drammer.Integration.Rss;

/// <summary>
/// The RSS reader interface.
/// </summary>
public interface IRssReader
{
    /// <summary>
    /// Fetches the RSS feed asynchronously.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="arguments">The arguments.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="Article"/>.</returns>
    Task<Result<List<Article>>> FetchAsync(
        DateTime? startDate = null,
        IDictionary<string, string>? arguments = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Fetch all items in an RSS feed.
    /// </summary>
    /// <param name="timeoutBetweenIterationsInMs">The timeout between iterations in milliseconds.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="Article"/>.</returns>
    [Obsolete]
    Task<Result<List<Article>>> FetchAllAsync(
        int timeoutBetweenIterationsInMs = 5000,
        CancellationToken cancellationToken = default);
}