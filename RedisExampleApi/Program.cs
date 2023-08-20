using Microsoft.EntityFrameworkCore;
using RedisExample.Api.Cache;
using RedisExample.Api.Repository;
using RedisExample.Api.Services;
using RedisExampleApi.Models;
using RedisExampleApi.Repository;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("MemoryDatabase");
});

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IProductRepository>(options =>
{
    var dbContext = options.GetRequiredService<AppDbContext>();
    var productRepository = new ProductRepository(dbContext);
    var redisService = options.GetRequiredService<RedisService>();
    return new ProductRepositoryWithCache(productRepository, redisService);
});

builder.Services.AddSingleton<RedisService>(options =>
{
    return new RedisService(builder.Configuration["CacheOptions:Url"]!);
});

builder.Services.AddSingleton<IDatabase>(options =>
{
    var redisService = options.GetRequiredService<RedisService>();
    return redisService.GetDb();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
