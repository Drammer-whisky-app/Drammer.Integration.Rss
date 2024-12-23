using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Drammer.Common;
using Drammer.Integration.Rss.Models;
using Edi.SyndicationFeed.ReaderWriter;
using Edi.SyndicationFeed.ReaderWriter.Rss;
using Microsoft.Extensions.Logging;

namespace Drammer.Integration.Rss;

/// <summary>
/// The RSS reader.
/// </summary>
public sealed partial class RssReader : IRssReader
{
    private readonly string _rssFeedUrl;
    private readonly IXmlReaderFactory _xmlReaderFactory;
    private readonly ILogger<RssReader> _logger;

    internal RssReader(string rssFeedUrl, IXmlReaderFactory xmlReaderFactory, ILogger<RssReader> logger)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rssFeedUrl);
        _rssFeedUrl = rssFeedUrl;
        _xmlReaderFactory = xmlReaderFactory;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result<List<Article>>> FetchAsync(
        DateTime? startDate = null,
        IDictionary<string, string>? arguments = null,
        CancellationToken cancellationToken = default)
    {
        using var reader = _xmlReaderFactory.Create(
            $"{_rssFeedUrl}{ParseArgumentsToQueryString(arguments)}",
            new XmlReaderSettings {Async = true});

        var rssParser = new RssParser();
        var feed = _xmlReaderFactory.CreateFeedReader(reader);

        var result = new List<Article>();
        var copyright = string.Empty;

        while (await feed.Read().ConfigureAwait(false))
        {
            if (feed.ElementType == SyndicationElementType.Item)
            {
                var rawElement = await feed.ReadElementAsString().ConfigureAwait(false);

                var item = rssParser.ParseItem(rawElement);
                var imgList = new List<string>();

                if (startDate.HasValue == false || item.Published.UtcDateTime >= startDate.Value)
                {
                    if (!string.IsNullOrWhiteSpace(rawElement))
                    {
                        var matches = ImageTagRegex().Matches(rawElement);
                        imgList.AddRange(
                            matches.Cast<Match>().Select(m => m.Groups[1].Value)
                                .Where(imgUrl => !string.IsNullOrWhiteSpace(imgUrl?.Trim())));
                    }

                    var article = new Article(
                        item.Id,
                        item.Title,
                        item.Published.UtcDateTime,
                        item.Description,
                        item.Links?.First().Uri.AbsoluteUri ?? string.Empty,
                        copyright,
                        item.Contributors.Select(x => new Author(x.Name, x.Email, x.Uri)),
                        item.Categories.Select(x => new Category(x.Name, x.Label, x.Scheme)),
                        new List<Link>(),
                        imgList);

                    result.Add(article);
                }
            }
            else if (feed.ElementType == SyndicationElementType.Content)
            {
                var item = await feed.ReadContent().ConfigureAwait(false);
                if (item.Name.Equals("copyright", StringComparison.OrdinalIgnoreCase))
                {
                    copyright = item.Value;
                }
            }
        }

        return Result.Success(result);
    }

    /// <inheritdoc />
    [Obsolete]
    public async Task<Result<List<Article>>> FetchAllAsync(
        int timeoutBetweenIterationsInMs = 5000,
        CancellationToken cancellationToken = default)
    {
        if (timeoutBetweenIterationsInMs < 0)
        {
            throw new ArgumentException(
                "The timeout between iterations should be a positive number.",
                nameof(timeoutBetweenIterationsInMs));
        }

        _logger.LogDebug("FetchAllAsync(): starting");

        var result = new List<Article>();

        Result<List<Article>>? fetchResult = null;
        var iteration = 1;
        var stop = false;
        do
        {
            try
            {
                fetchResult = await FetchAsync(
                    arguments: FetchAllArguments(iteration),
                    cancellationToken: cancellationToken).ConfigureAwait(false);

                if (fetchResult is {IsSuccess: true, Value: not null} && fetchResult.Value.Count != 0)
                {
                    // check if we found the same article again
                    if (result.Count != 0)
                    {
                        var title = result.Last().Title;
                        if (title != null && title.Equals(fetchResult.Value.Last().Title))
                        {
                            _logger.LogDebug(
                                "FetchAllAsync(): stopping because the same article is found twice");
                            stop = true;
                        }
                    }

                    if (!stop)
                    {
                        result.AddRange(fetchResult.Value);
                    }
                }
                else
                {
                    _logger.LogDebug("FetchAllAsync(): stopping because no articles were found");
                    stop = true;
                }
            }
            catch (WebException ex)
            {
                _logger.LogWarning(ex, "FetchAllAsync(): stopping because of error `{Message}`", ex.Message);
                stop = true;
            }

            iteration++;

            if (timeoutBetweenIterationsInMs > 0)
            {
                await Task.Delay(timeoutBetweenIterationsInMs, cancellationToken).ConfigureAwait(false);
            }
        } while (!stop && fetchResult != null && fetchResult is {IsSuccess: true, Value: not null}
                 && fetchResult.Value.Count != 0);

        _logger.LogDebug("FetchAllAsync(): done, found {ArticleCount} articles", result.Count);

        return Result.Success(result);
    }

    /// <summary>
    /// Gets additional arguments for the <see cref="FetchAllAsync"/> function per iteration.
    /// </summary>
    /// <param name="iteration">
    /// The iteration, starting by 1.
    /// </param>
    /// <returns>
    /// The <see cref="IDictionary{TKey, TValue}"/>.
    /// </returns>
    private IDictionary<string, string>? FetchAllArguments(int iteration)
    {
        return null;
    }

    /// <summary>
    /// Parses the arguments to a query string.
    /// </summary>
    /// <param name="arguments">
    /// The arguments.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    private static string ParseArgumentsToQueryString(IDictionary<string, string>? arguments)
    {
        if (arguments == null || arguments.Count == 0)
        {
            return string.Empty;
        }

        return $"?{string.Join("&", arguments.Select(x => $"{x.Key}={x.Value}"))}";
    }

    [GeneratedRegex("<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex ImageTagRegex();
}