using System;
using PL.Products.API.Commands;
using PL.Products.API.Queries;

namespace PL.Products.API;

internal static class Utils
{
    public static bool IsHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
        {
            return false;
        }

        var typeDefinition = type.GetGenericTypeDefinition();

        return typeDefinition == typeof(ICommandHandler<>)
            || typeDefinition == typeof(IQueryHandler<,>);
    }
}