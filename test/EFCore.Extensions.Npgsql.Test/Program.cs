using System.ComponentModel.DataAnnotations.Schema;
using EFCore.Extensions.Npgsql;
using Microsoft.EntityFrameworkCore;

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
            //todo: 插入多条时如何通过 Exception 定位触发异常的数据？
            //插入多条也是通过单条插入 INSERT INTO VALUES，但是 Entries 保存多条数据导致无法定位异常数据
            //INSERT INTO record(name, time)
            //VALUES($1, $2)
            //RETURNING id
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
    public DbSet<Record> Records { get; set; }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=EfCoreTest;Username=postgres;Password=123456;");
    }

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
