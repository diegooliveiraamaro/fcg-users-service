using Microsoft.EntityFrameworkCore;
using Users.Api.Domain.Entities;

namespace Users.Api.Infrastructure.Data
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
    }
}
