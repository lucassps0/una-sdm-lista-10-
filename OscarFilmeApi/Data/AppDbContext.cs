using Microsoft.EntityFrameworkCore;
using OscarFilmeApi.Models;

namespace OscarFilmeApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Filme> Filmes => Set<Filme>();
}
