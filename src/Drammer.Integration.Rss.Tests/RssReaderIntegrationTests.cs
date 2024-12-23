using Microsoft.Extensions.Logging;

namespace Drammer.Integration.Rss.Tests;

public sealed class RssReaderIntegrationTests
{
    [Fact]
    public async Task FetchAsync_IntegrationTest1()
    {
        // Arrange
        var rssReader = new RssReader(
            "Drammer.Integration.Rss.Tests.Resources.IntegrationTest1.xml",
            new ResourceXmlReaderFactory(),
            Mock.Of<ILogger<RssReader>>());

        // Act
        var result = await rssReader.FetchAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(3);

        var firstItem = result.Value![0];
        firstItem.Title.Should().Be("Dummy Item Title 1");
        firstItem.Summary.Should().Be("Dummy description for item 1.");
        firstItem.Images.Should().HaveCount(0);
        firstItem.Published.Should().Be(new DateTime(2024, 12, 23, 8, 6, 18));
        firstItem.Url.Should().Be("https://example.com/dummy-link-1");
        firstItem.Copyright.Should().BeNullOrEmpty();
        firstItem.Authors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task FetchAsync_IntegrationTest2()
    {
        // Arrange
        var rssReader = new RssReader(
            "Drammer.Integration.Rss.Tests.Resources.IntegrationTest2.xml",
            new ResourceXmlReaderFactory(),
            Mock.Of<ILogger<RssReader>>());

        // Act
        var result = await rssReader.FetchAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(10);

        var firstItem = result.Value![0];
        firstItem.Title.Should().Be("Dummy Item 1");
        firstItem.Summary.Should().Be("<img src=\"/path/to/dummy-image.jpg\"/><br />Dummy description for item 1");
        firstItem.Images.Should().Contain("/path/to/dummy-image.jpg");
        firstItem.Published.Should().Be(new DateTime(2024, 12, 23, 16, 0, 0));
        firstItem.Url.Should().Be("https://example.com/dummy-link-1");
        firstItem.Copyright.Should().BeNullOrEmpty();
        firstItem.Authors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task FetchAsync_IntegrationTest3()
    {
        // Arrange
        var rssReader = new RssReader(
            "Drammer.Integration.Rss.Tests.Resources.IntegrationTest3.xml",
            new ResourceXmlReaderFactory(),
            Mock.Of<ILogger<RssReader>>());

        // Act
        var result = await rssReader.FetchAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(3);

        var firstItem = result.Value![0];
        firstItem.Title.Should().Be("Dummy Item Title 1");
        firstItem.Summary.Should().Be("Dummy description for item 1.");
        firstItem.Images.Should().HaveCount(0);
        firstItem.Published.Should().Be(new DateTime(2024, 12, 5, 18, 38, 0));
        firstItem.Url.Should().Be("https://example.com/dummy-item-1");
        firstItem.Copyright.Should().BeNullOrEmpty();
        firstItem.Authors.Should().BeNullOrEmpty();
        firstItem.Id.Should().Be("tag:example.com,1999:blog-dummy-12345.post-1");
        firstItem.Categories.Should().HaveCount(1);
    }
}