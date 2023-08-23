// See https://aka.ms/new-console-template for more information

using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using EFCore.Extensions.SqlServer;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

var dbContext = new TestDbContext();
var tran = dbContext.Database.BeginTransaction();

var r = new Record { Name = "abc1      " };
var r2 = new Record { Name = "abc4      " };
var r3 = new Record { Name = "xx" };

//dbContext.Records.AddRange(r, r2, r3);

var list = new List<Record> { r, r2, r3 };
for (int i = 0; i < 10; i++)
{
    list.Add(new Record { Name = i.ToString() });
}

dbContext.AddRange(list);

while (true)
{
    try
    {
        dbContext.SaveChanges(false);
        tran.Commit();
        //tran.Rollback();

        break;
    }
    catch (DbUpdateException ex)
    {
        if (ex.IsDuplicateKeyError())
        {
            //插入数据小于三个，单条插入，此时可以处理异常
            //INSERT INTO[record] ([name], [time])
            //VALUES(@p0, @p1);
            //SELECT[id]
            //FROM[record]
            //WHERE @@ROWCOUNT = 1 AND[id] = scope_identity();
            if (ex.Entries.Count() == 1)
            {
                Debug.WriteLine(ex.Entries[0].Entity);
                ex.Entries[0].State = EntityState.Detached;
            }

            //todo: 插入多条时如何通过 Exception 定位触发异常的数据？
            //插入数据大于三个使用 MERGE，Entries多条无法定位异常数据
            //MERGE[record] USING(
            //    VALUES(@p0, @p1, 0),
            //    (@p2, @p3, 1),
            //    (@p4, @p5, 2),
            //    (@p6, @p7, 3)) AS i([name], [time], _Position) ON 1 = 0
            //WHEN NOT MATCHED THEN
            //INSERT([name], [time])
            //VALUES(i.[name], i.[time])
            //OUTPUT INSERTED.[id], i._Position
            //INTO @inserted0;
            //foreach (var entry in ex.Entries)
            //{
            //    Debug.WriteLine(entry.Entity);
            //    entry.State = EntityState.Detached;
            //}

            continue;
        }

        throw;
    }
}

tran.Dispose();

public class TestDbContext : DbContext
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=.\sql2017;Initial Catalog=EfCoreTest;User ID=sa;Password=123456");
    }

    public DbSet<Record> Records { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Record>().HasIndex(x => x.Name).IsUnique();
    }
}

[Table("record")]
public class Record
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("time")]
    public int Time { get; set; }
}
