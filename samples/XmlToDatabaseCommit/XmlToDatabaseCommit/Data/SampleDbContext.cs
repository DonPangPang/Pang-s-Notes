using Microsoft.EntityFrameworkCore;

namespace XmlToDatabaseCommit.Data;

/// <summary>
/// 
/// </summary>
public class SampleDbContext : DbContext
{
    public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //var xmlCommentHelper = new XmlCommentHelper();
        //xmlCommentHelper.LoadAll();

        //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        //{
        //    var typeComment = xmlCommentHelper.GetTypeComment(entityType.ClrType);
        //    modelBuilder.Entity(entityType.ClrType).ToTable(t => t.HasComment(typeComment));
        //    foreach (var property in entityType.ClrType.GetProperties().Where(x => x.IsPubliclyWritable()))
        //    {
        //        var propertyComment = xmlCommentHelper.GetFieldOrPropertyComment(property);
        //        modelBuilder.Entity(entityType.ClrType).Property(property.Name).HasComment(propertyComment);
        //    }
        //}

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
    [Comment("姓名")]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; } = string.Empty;
}