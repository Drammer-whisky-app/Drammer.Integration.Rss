using System.Reflection;
using System.Xml;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;

namespace Drammer.Integration.Rss.Tests;

internal sealed class ResourceXmlReaderFactory : IXmlReaderFactory
{
    public XmlReader Create(string inputUri, XmlReaderSettings? settings = null)
    {
        var assemlby = Assembly.GetExecutingAssembly();
        var stream = assemlby.GetManifestResourceStream(inputUri);
        return XmlReader.Create(stream, settings);
    }

    public ISyndicationFeedReader CreateFeedReader(XmlReader reader) => new RssFeedReader(reader);

}