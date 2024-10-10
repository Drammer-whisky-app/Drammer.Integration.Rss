using Drammer.Common;
using Drammer.Integration.Rss.Models;

namespace Drammer.Integration.Rss;

public interface IRssReader
{
    Task<Result<List<Article>>> FetchAsync(
        DateTime? startDate = null,
        IDictionary<string, string>? arguments = null,
        CancellationToken cancellationToken = default);

    Task<Result<List<Article>>> FetchAllAsync(
        int timeoutBetweenIterationsInMs = 5000,
        CancellationToken cancellationToken = default);
}