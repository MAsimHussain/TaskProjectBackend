using ApplicationLayer.Repository.Implementation;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using ServiceLayer.Service.Implementation;
using ServiceLayer.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<ApplicatonDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();   

app.UseHttpsRedirection();
app.UseCors();  
app.UseAuthorization();

app.MapControllers();

app.Run();
