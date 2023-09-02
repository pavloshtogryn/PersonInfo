using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PersonInfo.Models;

namespace PersonInfo
{
    public class PersonInfoDbContext : DbContext
    {
        public PersonInfoDbContext()
        {

        }
        public PersonInfoDbContext(DbContextOptions<PersonInfoDbContext> options) : base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(p => p.Id).ValueGeneratedOnAdd()
    .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        }
        public virtual DbSet<User> Users { get; set; }
    }
}
