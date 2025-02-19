
using System;
using System.Linq;
using System.Reflection;
using PL.Products.API.Entities;

namespace PL.Products.API;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<Dispatcher>();
        return services;
    }

    public static IServiceCollection AddMessageHandlers(this IServiceCollection services, Assembly assembly)
    {
        var assemblyTypes = assembly.GetTypes();

        foreach (var type in assemblyTypes)
        {
            var handlerInterfaces = type.GetInterfaces()
               .Where(Utils.IsHandlerInterface)
               .ToList();

            if (handlerInterfaces.Any())
            {
                var handlerFactory = new HandlerFactory(type);
                foreach (var interfaceType in handlerInterfaces)
                {
                    services.AddTransient(interfaceType, provider => handlerFactory.Create(provider, interfaceType));
                }
            }
        }

        var aggregateRootTypes = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Entity<Guid>)) && x.GetInterfaces().Contains(typeof(IAggregateRoot))).ToList();

        Type[] genericHandlerTypes = [];

        foreach (var aggregateRootType in aggregateRootTypes)
        {
            foreach (var genericHandlerType in genericHandlerTypes)
            {
                var handlerType = genericHandlerType.MakeGenericType(aggregateRootType);
                var handlerInterfaces = handlerType.GetInterfaces();

                var handlerFactory = new HandlerFactory(handlerType);
                foreach (var interfaceType in handlerInterfaces)
                {
                    services.AddTransient(interfaceType, provider => handlerFactory.Create(provider, interfaceType));
                }
            }
        }

        Dispatcher.RegisterEventHandlers(assembly, services);

        return services;
    }
}
