using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Interfaces;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Orders do not matter here, so feel free to add new services as needed.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddCors();

//above this line is service configuration
var app = builder.Build();

//below this line is middleware; orders matter here, so be careful when adding new middleware
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(policy => policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:4200","https://localhost:4200"));

// Configure the HTTP request pipeline.
app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    throw;
}

app.Run();
