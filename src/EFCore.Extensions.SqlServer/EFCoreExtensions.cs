using EFCore.Extensions.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.Npgsql;

public static class EFCoreExtensions
{
    public static bool IsDuplicateKeyError(this DbUpdateException exception) =>
        new SqlServerDbExceptionStrategy().IsDuplicateKeyError(exception);

    public static bool IsIncrementOverflowError(this DbUpdateException exception) =>
        new SqlServerDbExceptionStrategy().IsIncrementOverflowError(exception);
}
