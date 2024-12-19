using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TrustWaveCarca.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // Correct class declaration
    {
         public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

         public DbSet<UserLoginCredentials> UserLoginCredentials { get; set; }
         public DbSet<UserLoginLoges> LoginLoges { get; set; }
         public DbSet<ChatRequest> ChatRequest { get; set; }

        public DbSet<PartnerChat> PartnerChat { get; set; }
      
        public DbSet<ChatMessage> ChatMessages { get; set; }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UserLoginCredentials (optional: e.g., DateOnly handling)
            modelBuilder.Entity<UserLoginCredentials>()
                .Property(ulc => ulc.RegistrationDate)
                .HasConversion(
                    v => v.ToString(),  // Convert DateOnly to string
                    v => DateOnly.Parse(v)  // Convert string back to DateOnly
                );
                      
            modelBuilder.Entity<UserLoginCredentials>()
                       .HasIndex(u => u.UniqueLoginID)
                       .IsUnique();

            modelBuilder.Entity<ChatMessage>()
                .HasKey(m => m.Id); // Define Id as the primary key
            modelBuilder.Entity<ChatMessage>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd(); // Auto-increment
            modelBuilder.Entity<ChatMessage>()
                .Property(m => m.Timestamp)
                .HasColumnType("timestamp with time zone");




        }
    }
}
