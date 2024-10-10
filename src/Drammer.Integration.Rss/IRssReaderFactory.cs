namespace Drammer.Integration.Rss;

public interface IRssReaderFactory
{
    IRssReader Create(string rssFeedUrl);
}