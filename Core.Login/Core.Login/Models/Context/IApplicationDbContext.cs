using Microsoft.EntityFrameworkCore;

namespace Core.Login.Models.Context
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }

        bool SaveChanges();
    }
}