using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SisandBackend.Entities.Users;

namespace SisandBackend.EntityFrameworkCore.Contexts;

public class SisandDbContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    #region DbSet

    public DbSet<User> Users { get; set; }

    #endregion

    #region Construtors

    public SisandDbContext(DbContextOptions<SisandDbContext> options): base(options){}

    #endregion

    #region Overrides

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.IsDeleted).HasDefaultValue(false);
            entity.HasQueryFilter(u => !u.IsDeleted);
        });
    }

    #endregion
}
