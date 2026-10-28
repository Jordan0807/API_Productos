public class appDbContext : DbContext
{
    public appDbContext(DbContextOptions<appDbContext> options) : base(options)
    {
    }

    public DbSet<Producto> Productos { get; set; }
}