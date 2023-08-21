using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.Common;

public interface IDbExceptionStrategy
{
    /// <summary>
    /// Determines whether exception caused by 重复键违反唯一约束 or 违反了 PRIMARY KEY 约束.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public bool IsDuplicateKeyError(DbUpdateException exception);

    /// <summary>
    /// Determines whether exception caused by 自增主键超过最大值.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public bool IsIncrementOverflowError(DbUpdateException exception);
}
