using System;
using Blackmaw.Dal.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blackmaw.Dal.DbContext
{
    public class BmDbContext : IdentityDbContext<BmUser, BmRole, Guid>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Character> Characters { get; set; }
        
        public BmDbContext(DbContextOptions<BmDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.;database=blackmaw.dev;Integrated Security=True;Persist Security Info=True");
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Game>(entity =>
            {
                entity.HasOne(e => e.RuleSystem)
                    .WithMany(e => e.Games)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<RuleSystem>(entity => 
            {
            });


            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
