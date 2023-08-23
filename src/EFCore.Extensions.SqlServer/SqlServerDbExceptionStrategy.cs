using EFCore.Extensions.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SqlServer;

public class SqlServerDbExceptionStrategy : IDbExceptionStrategy
{
    /// <inheritdoc />
    public bool IsDuplicateKeyError(DbUpdateException exception)
    {
        if (exception.InnerException is SqlException ex)
        {
            switch (ex.Number)
            {
                //duplicate value in a column with a unique constraint, 唯一约束
                case 2627:
                //duplicate primary key value, PRIMARY KEY 约束
                case 2601:
                    return true;
            }
        }

        return false;
    }

    /// <inheritdoc />
    public bool IsIncrementOverflowError(DbUpdateException exception)
    {
        if (exception.InnerException is SqlException ex)
        {
            switch (ex.Number)
            {
                //将IDENTITY转换为数据类型int时出现算术溢出错误。
                case 8115:
                    return true;
            }
        }

        return false;
    }
}
