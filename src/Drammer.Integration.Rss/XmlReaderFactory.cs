using System.Diagnostics.CodeAnalysis;
using System.Xml;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;

namespace Drammer.Integration.Rss;

[ExcludeFromCodeCoverage]
internal sealed class XmlReaderFactory : IXmlReaderFactory
{
    public XmlReader Create(string inputUri, XmlReaderSettings? settings = null) =>
        XmlReader.Create(inputUri, settings);

    public ISyndicationFeedReader CreateFeedReader(XmlReader reader) => new RssFeedReader (reader);
}