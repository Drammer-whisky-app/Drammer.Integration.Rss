using System.Xml;
using Microsoft.SyndicationFeed;

namespace Drammer.Integration.Rss;

/// <summary>
/// The XML reader factory interface.
/// </summary>
public interface IXmlReaderFactory
{
    /// <summary>
    /// Creates an instance of <see cref="XmlReader"/>.
    /// </summary>
    /// <param name="inputUri">The input URI.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>An <see cref="XmlReader"/>.</returns>
    XmlReader Create(string inputUri, XmlReaderSettings? settings = null);

    /// <summary>
    /// Creates an instance of <see cref="ISyndicationFeedReader"/>.
    /// </summary>
    /// <param name="reader">The XML reader.</param>
    /// <returns>A <see cref="ISyndicationFeedReader"/>.</returns>
    ISyndicationFeedReader CreateFeedReader(XmlReader reader);
}