using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Primary Key
        modelBuilder.Entity<User>()
            .HasKey(x => x.Id)
            .HasName("PK_User");
        
        modelBuilder.Entity<Note>()
            .HasKey(x => x.Id)
            .HasName("PK_Note");
        
        // Auto Increment ID
        modelBuilder.Entity<User>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Note>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        // Unique
        modelBuilder.Entity<User>()
            .Property(x => x.Username)
            .IsUnicode();
        
        // Relations
        // One-To-Many
        modelBuilder.Entity<Note>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    public DbSet<User> UsersTable { get; set; }
    public DbSet<Note> NotesTable { get; set; }
}