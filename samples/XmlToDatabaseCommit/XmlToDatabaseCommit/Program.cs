using Microsoft.EntityFrameworkCore;
using XmlToDatabaseCommit.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SampleDbContext>(opts =>
{
    opts.UseSqlite("Data Source=sample.db");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await using var scoped = app.Services.CreateAsyncScope();
using var db = scoped.ServiceProvider.GetRequiredService<SampleDbContext>();
try
{
    db.Database.Migrate();
    //db.Database.EnsureDeleted();
    //db.Database.EnsureCreated();
}
catch
{
    Console.WriteLine("seed error.");
}

app.UseAuthorization();

app.MapControllers();

app.Run();
