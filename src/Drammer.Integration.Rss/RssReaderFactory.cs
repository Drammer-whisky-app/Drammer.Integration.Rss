using Microsoft.Extensions.Logging;

namespace Drammer.Integration.Rss;

internal sealed class RssReaderFactory : IRssReaderFactory
{
    private readonly IXmlReaderFactory _xmlReaderFactory;
    private readonly ILoggerFactory _loggerFactory;

    public RssReaderFactory(IXmlReaderFactory xmlReaderFactory, ILoggerFactory loggerFactory)
    {
        _xmlReaderFactory = xmlReaderFactory;
        _loggerFactory = loggerFactory;
    }

    public IRssReader Create(string rssFeedUrl) => new RssReader(
        rssFeedUrl,
        _xmlReaderFactory,
        _loggerFactory.CreateLogger<RssReader>());
}