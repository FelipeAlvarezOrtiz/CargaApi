using CargaBd.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CargaBd.API.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        
    }
}
