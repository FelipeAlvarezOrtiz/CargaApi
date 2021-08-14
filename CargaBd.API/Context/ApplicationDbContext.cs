using CargaBd.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CargaBd.API.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Payload> Payload { get; set; }
        public DbSet<SkillOptional> SkillsOptionals { get; set; }
        public DbSet<SkillRequired> SkillsRequired { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
