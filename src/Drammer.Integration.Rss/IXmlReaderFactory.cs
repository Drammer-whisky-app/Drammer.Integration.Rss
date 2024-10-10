using System.Xml;
using Microsoft.SyndicationFeed;

namespace Drammer.Integration.Rss;

public interface IXmlReaderFactory
{
    XmlReader Create(string inputUri, XmlReaderSettings? settings = null);

    ISyndicationFeedReader CreateFeedReader(XmlReader reader);
}