using Microsoft.Extensions.Logging;

namespace Drammer.Integration.Rss.Tests;

public sealed class RssReaderFactoryTests
{
    [Fact]
    public void Create_ReturnsRssReader()
    {
        // arrange
        var loggerFactory = new Mock<ILoggerFactory>();
        var xmlReaderFactory = new Mock<IXmlReaderFactory>();
        var factory = new RssReaderFactory(xmlReaderFactory.Object, loggerFactory.Object);

        // act
        var reader = factory.Create("test");

        // assert
        reader.Should().NotBeNull();
        reader.Should().BeOfType<RssReader>();
    }
}