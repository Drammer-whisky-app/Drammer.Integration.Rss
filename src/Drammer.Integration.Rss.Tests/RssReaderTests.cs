using System.Xml;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SyndicationFeed;

namespace Drammer.Integration.Rss.Tests;

public sealed class RssReaderTests
{
    [Fact]
    public async Task FetchAsync_WithValidUrl_ReturnsRssFeed()
    {
        // arrange
        const string RawElement =
            "<item>\n\t\t\t<title>\n\t\t\t\t<![CDATA[Title!]]>\n\t\t\t</title>\n\t\t\t<link>https://drammer.com/test</link>\n\t\t\t<description>\n\t\t\t\t<![CDATA[<img src=\"/get/files/image/my-image.jpg\"/><br />Test description.]]>\n\t\t\t</description>\n\t\t\t<pubDate>Wed, 09 Oct 2024 07:22:02 -0700</pubDate>\n\t\t</item>";

        var xmlReader = CreateReader();
        var feedReader = CreateFeedReader(RawElement);

        var xmlReaderFactory = new Mock<IXmlReaderFactory>();
        xmlReaderFactory.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<XmlReaderSettings?>())).Returns(xmlReader);
        xmlReaderFactory.Setup(x => x.CreateFeedReader(It.IsAny<XmlReader>())).Returns(feedReader);

        var url = "https://localhost/rss.xml";
        var reader = new RssReader(url, xmlReaderFactory.Object, NullLogger<RssReader>.Instance);

        // act
        var feed = await reader.FetchAsync();

        // assert
        feed.Should().NotBeNull();
        feed.IsSuccess.Should().BeTrue();

        var item = feed.Value!.First();
        item.Title.Should().Be("Title!");
        item.Summary.Should().Be("<img src=\"/get/files/image/my-image.jpg\"/><br />Test description.");
        item.Images.Should().HaveCount(1);
        item.Images.First().Should().Be("/get/files/image/my-image.jpg");
    }

    private static XmlReader CreateReader()
    {
        var reader = new Mock<XmlReader>();
        return reader.Object;
    }

    private static ISyndicationFeedReader CreateFeedReader(string rawElement)
    {
        var reader = new Mock<ISyndicationFeedReader>();

        // one iteration
        reader.SetupSequence(x => x.Read()).ReturnsAsync(true).ReturnsAsync(false);

        reader.SetupGet(x => x.ElementType).Returns(SyndicationElementType.Item);

        reader.Setup(x => x.ReadElementAsString()).ReturnsAsync(
            rawElement);

        return reader.Object;
    }
}