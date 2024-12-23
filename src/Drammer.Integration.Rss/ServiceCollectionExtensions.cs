using Microsoft.Extensions.DependencyInjection;

namespace Drammer.Integration.Rss;

/// <summary>
/// The service collection extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the XML reader factory and RSS reader factory to the service collection.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddRssReaderFactory(this IServiceCollection services)
    {
        services.AddSingleton<IXmlReaderFactory, XmlReaderFactory>();
        services.AddSingleton<IRssReaderFactory, RssReaderFactory>();
        return services;
    }
}