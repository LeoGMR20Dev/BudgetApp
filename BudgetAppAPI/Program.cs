using BudgetAppAPI.AutoMappers;
using BudgetAppAPI.Contexts;
using BudgetAppAPI.DTOs.BudgetTransactions;
using BudgetAppAPI.Interfaces.Repositories;
using BudgetAppAPI.Interfaces.Services;
using BudgetAppAPI.Middlewares;
using BudgetAppAPI.Models;
using BudgetAppAPI.Repositories;
using BudgetAppAPI.Services;
using BudgetAppAPI.Validators.BudgetTransactions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BudgetContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
});

builder.Services.AddScoped<IValidator<AddBudgetTransaction>, AddBudgetTransactionValidator>();

//Services

builder.Services.AddScoped<IBudgetService<BudgetTransactionForList, BudgetDataDto, AddBudgetTransaction>, BudgetService>();

//Repositories

builder.Services.AddScoped<IBudgetRepository<BudgetTransaction>, BudgetRepository>();

//Mappers

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<BudgetTransactionMappingProfile>();
});

builder.Services.AddCors(x => x.AddPolicy("CorsPolicy", options =>
{
    options.WithOrigins(builder.Configuration["AppSettings:Audience"])
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();
