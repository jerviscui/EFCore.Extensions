using EFCore.Extensions.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EFCore.Extensions.Npgsql;

public static class ServiceCollectionExtensions
{
    public static void UsePostgreSqlStrategy(this IServiceCollection services)
    {
        services.TryAddSingleton<IDbExceptionStrategy, PostgreSqlDbExceptionStrategy>();
    }
}
