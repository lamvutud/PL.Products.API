using RazorLight;
using System;

namespace PL.Products.API;

public static class HtmlCollectionExtensions
{
    public static IServiceCollection AddHtmlRazorLightEngine(this IServiceCollection services)
    {
        var engine = new RazorLightEngineBuilder()
               .UseFileSystemProject(Environment.CurrentDirectory)
               .UseMemoryCachingProvider()
               .Build();

        services.AddSingleton<IRazorLightEngine>(engine);

        return services;
    }
}
