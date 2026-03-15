using BudgetAppAPI.AutoMappers;
using BudgetAppAPI.Contexts;
using BudgetAppAPI.DTOs.BudgetTransactions;
using BudgetAppAPI.Interfaces.Repositories;
using BudgetAppAPI.Interfaces.Services;
using BudgetAppAPI.Models;
using BudgetAppAPI.Repositories;
using BudgetAppAPI.Services;
using Microsoft.EntityFrameworkCore;
using BudgetAppAPI.Middlewares;

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

//Services

builder.Services.AddScoped<IBudgetService<BudgetTransactionForList, BudgetDataDto, AddBudgetTransaction>, BudgetService>();

//Repositories

builder.Services.AddScoped<IBudgetRepository<BudgetTransaction>, BudgetRepository>();

//Mappers

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<BudgetTransactionMappingProfile>();
});

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

app.Run();
