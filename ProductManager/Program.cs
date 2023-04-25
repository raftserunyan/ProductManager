using Microsoft.EntityFrameworkCore;
using ProductManager.API.Extensions;
using ProductManager.Core.Extensions;
using ProductManager.Data.DAO;
using ProductManager.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddUnitOfWork();
builder.Services.AddServices();

// Configure DbContext
builder.Services.AddDbContext<ProductManagerDbContext>(
        opts => opts.UseSqlServer(builder.Configuration["ConnectionStrings"]),
        ServiceLifetime.Scoped
       );

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseCustomExceptionHandling();

app.UseAuthorization();

app.MapControllers();

app.Run();
