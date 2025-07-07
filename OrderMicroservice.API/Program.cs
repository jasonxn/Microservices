using Microsoft.EntityFrameworkCore;
using OrderMicroservice.API.Data;
using System;
using Microsoft.EntityFrameworkCore;
using OrderMicroservice.API.Clients;
using Microsoft.Extensions.DependencyInjection;
using OrderMicroservice.API.Repositories;
using OrderMicroservice.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<ProductClientOptions>(
    builder.Configuration.GetSection("ProductService"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IProductClient, ProductClient>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
