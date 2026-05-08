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
}