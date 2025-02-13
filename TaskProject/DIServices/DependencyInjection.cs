using ApplicationLayer.Repository.Implementation;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using ServiceLayer.Service.Implementation;
using ServiceLayer.Services.Interface;

namespace TaskProject.UI.DIServices
{
	public static class DependencyInjection
	{



		public  static void SqlConnection(this IServiceCollection service, IConfiguration builder)
		{

			service.AddDbContext<ApplicatonDbContext>(options => options.UseSqlServer(builder.GetConnectionString("DefaultConnection")));

		}
		public  static void RegisterDIServices(this IServiceCollection service, IConfiguration builder)
		{

			service.AddTransient<IEmployeeService, EmployeeService>();
			service.AddTransient<IFileService, FileService>();

		}
		
		public  static void RegisterCorsPolicy(this IServiceCollection service, IConfiguration builder)
		{

			service.AddCors(option =>
			{
				option.AddDefaultPolicy(builder =>
				{
					builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
				});
			});

		}
	}
}
