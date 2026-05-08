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