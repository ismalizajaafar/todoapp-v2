using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;
using TodoApp.Core.Interfaces;
using TodoApp.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS - Allow Vercel and localhost
var allowedOrigins = new[] 
{
    "https://your-todoapp-frontend.vercel.app",  // We'll update this after deployment
    "http://localhost:3000"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowProduction", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Database - Use SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=todoapp.db"));

// Manual service registration
builder.Services.AddScoped<ITodoRepository, TodoApp.Infrastructure.Repositories.TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowProduction");
app.UseAuthorization();
app.MapControllers();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    
    // Add sample data if empty
    if (!context.TodoItems.Any())
    {
        context.TodoItems.AddRange(
            new TodoApp.Core.Entities.TodoItem 
            { 
                Title = "Welcome to TodoApp!", 
                Description = "This is your first todo", 
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new TodoApp.Core.Entities.TodoItem 
            { 
                Title = "Deployment successful", 
                Description = "Your app is live!", 
                IsCompleted = true,
                CreatedAt = DateTime.UtcNow
            }
        );
        context.SaveChanges();
    }
}

app.Run();