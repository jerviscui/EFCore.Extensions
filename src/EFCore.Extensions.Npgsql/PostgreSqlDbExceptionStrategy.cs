using EFCore.Extensions.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EFCore.Extensions.Npgsql;

public class PostgreSqlDbExceptionStrategy : IDbExceptionStrategy
{
    /// <inheritdoc />
    public bool IsDuplicateKeyError(DbUpdateException exception)
    {
        // a unique constraint
        return exception.InnerException is PostgresException { SqlState: "23505" };
    }

    /// <inheritdoc />
    public bool IsIncrementOverflowError(DbUpdateException exception)
    {
        //达到序列的最大值了 (2147483647)
        return exception.InnerException is PostgresException { SqlState: "2200H" };
    }
}
