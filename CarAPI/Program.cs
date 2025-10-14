using Microsoft.EntityFrameworkCore;
using CarAPI.Data;
using CarAPI.Repositories;
using CarAPI.Models;
using CarAPI.Services;
using CarAPI.DTOs;
using CarAPI.Mapping;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(opt => opt.AddProfile<MappingProfile>());


// Add repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICarRepository, CarRepository>();

// Add services
builder.Services.AddScoped<EntityService<CarBrand, EntityDto, CreateEntityDto>>();
builder.Services.AddScoped<EntityService<TrimLevel, EntityDto, CreateEntityDto>>();
builder.Services.AddScoped<CarService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

app.Run();