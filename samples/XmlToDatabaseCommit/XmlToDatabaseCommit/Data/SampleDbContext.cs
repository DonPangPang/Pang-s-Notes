using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;

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
}