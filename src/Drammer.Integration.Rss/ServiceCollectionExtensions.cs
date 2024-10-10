using Microsoft.Extensions.DependencyInjection;

namespace Drammer.Integration.Rss;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRssReaderFactory(this IServiceCollection services)
    {
        services.AddSingleton<IXmlReaderFactory, XmlReaderFactory>();
        services.AddSingleton<IRssReaderFactory, RssReaderFactory>();
        return services;
    }
}