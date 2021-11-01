using DisneyAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace DisneyAPI.Contexts
{
    public class DisneyContext:DbContext
    {
        private const string Schema = "disney";
        public DisneyContext(DbContextOptions<DisneyContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(Schema);
        }
        public DbSet<Character> Characters { get; set; } = null!;
        public DbSet<Genre> Genders { get; set; } = null!;
        public DbSet<MovieOrSerie> MovieOrSeries { get; set; } = null!;
    }
}
