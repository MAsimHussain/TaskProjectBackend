using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using ServiceLayer.Service.Implementation;
using ServiceLayer.Services.Interface;
using TaskProject.UI.DIServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.SqlConnection(builder.Configuration);

builder.Services.RegisterDIServices(builder.Configuration);

builder.Services.RegisterCorsPolicy(builder.Configuration);
builder.Services.AddMemoryCache();  
builder.Services.AddLazyCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Serve Static files
app.UseStaticFiles();   

app.UseHttpsRedirection();
app.UseCors();  
app.UseAuthorization();

app.MapControllers();

app.Run();
