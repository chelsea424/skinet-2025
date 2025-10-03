using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//above this line is service configuration
var app = builder.Build();

//below this line is middleware

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
