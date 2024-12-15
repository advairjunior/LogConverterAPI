using LogConverterAPI.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LogConverterAPI.Data;

public class LogContext(DbContextOptions<LogContext> options) : DbContext(options)
{
    public DbSet<Log> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Log>().ToTable("Logs");
        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.LogFormatoCDN)
                  .IsRequired();

            entity.Property(e => e.LogFormatoAgora)
                  .IsRequired();
        });
    }
}
