using Microsoft.EntityFrameworkCore;
using ProyectoIntuit.Models;

namespace ProyectoIntuit.Context
{
    public class AppDbContext: DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<Cliente> Cliente { get; set; }

        public async Task<List<Cliente>> GetAllClientes_SP()
        {
            return await Cliente
                .FromSqlRaw("EXEC dbo.GetAllClientes")
                .ToListAsync();
        }
    }
}
