using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.Npgsql;

public static class EFCoreExtensions
{
    public static bool IsDuplicateKeyError(this DbUpdateException exception) =>
        new PostgreSqlDbExceptionStrategy().IsDuplicateKeyError(exception);

    public static bool IsIncrementOverflowError(this DbUpdateException exception) =>
        new PostgreSqlDbExceptionStrategy().IsIncrementOverflowError(exception);
}
