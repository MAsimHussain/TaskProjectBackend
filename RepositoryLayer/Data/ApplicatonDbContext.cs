using DomainLayer.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Data
{
    public class ApplicatonDbContext:DbContext  
    
    {
        public ApplicatonDbContext(DbContextOptions<ApplicatonDbContext> options):base(options)
        {
            
        }


        public DbSet<Employee> tblEmployees { get; set; }  
    }
}
