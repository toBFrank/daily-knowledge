using DailyKnowledge.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// - builder is the setup object for all of app's configs/services
builder.Services.AddControllers();
// - builder.Services.AddDbContext is just instructions on how to make a dbcontext
// TODO: Switch to SQL Server database when deploying to production
builder.Services.AddDbContext<DailyKnowledgeDbContext>(opt =>
    opt.UseInMemoryDatabase("DailyKnowledgeDbInMemory")
);
builder.Services.AddOpenApi();

// CORS policy
builder.Services.AddCors(options => 
{
    options.AddPolicy("Frontend", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// - builder.Build() builds app and takes builder.Services 
//   to create actual Dependency Injection container (IServiceProvider))
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
