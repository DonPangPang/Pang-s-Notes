using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XmlToDatabaseCommit.Data;

[Table("__EFMigrationsHistory")]
public class EFMigrationsHistory
{
    [Key]
    [MaxLength(150)]
    public required string MigrationId { get; set; }

    [MaxLength(32)]
    public string ProductVersion { get; set; } = null!;

    [NotMapped]
    public string Sort => MigrationId.Split("_")[0];

    [Comment("迁移时间")]
    public DateTime? MigrationTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 默认成功，失败不会入库
    /// </summary>
    [Column(TypeName = "varchar(20)")]
    public MigrationType MigrationType { get; set; } = MigrationType.Success;
}

public enum MigrationType
{
    Success
}

/// <summary>
/// 
/// </summary>
public class SampleDbContext : DbContext
{
    public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options) { }

    public DbSet<EFMigrationsHistory> EFMigrationsHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        var xmlCommentHelper = new XmlCommentHelper();
        xmlCommentHelper.LoadAll();

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var typeComment = xmlCommentHelper.GetTypeComment(entityType.ClrType);
            modelBuilder.Entity(entityType.ClrType).ToTable(t => t.HasComment(typeComment));
            foreach (var property in entityType.ClrType.GetProperties().Where(x => x.IsPubliclyWritable()))
            {
                var propertyComment = xmlCommentHelper.GetFieldOrPropertyComment(property);
                modelBuilder.Entity(entityType.ClrType).Property(property.Name).HasComment(propertyComment);
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}

/// <summary>
/// 用户表
/// </summary>
public class User
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 姓名
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// 邮箱
    /// </summary>
    public required string Email { get; set; }

    public ICollection<Company> Companies { get; set; } = new List<Company>();
}

public class Company
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}

public class SqlQueryTest(SampleDbContext dbContext)
{
    public IQueryable<T> Query<T>() where T : class
    {


        return dbContext.Database.SqlQuery<T>($"select * from {typeof(T).Name}");
    }

    public async Task Test<T>() where T : class
    {
        var user = await dbContext.Set<User>().SingleAsync();
    }
}