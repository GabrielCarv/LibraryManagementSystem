using Microsoft.EntityFrameworkCore;
using Library_Management_System.Models;
using Library_Management_System.Extensions;
using Library_Management_System.Maps;

namespace Library_Management_System.Data
{
    public class LibraryContext : DbContext
    {

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public IConfiguration Configuration { get; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            string connectionString = @"Server=SARAH\SQLEXPRESS01;Database=LMSdatabase;Trusted_Connection=True;";
            options.UseSqlServer(connectionString);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.SetAuditProperties();
            return await base.SaveChangesAsync(cancellationToken);
        }
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ChangeTracker.SetAuditProperties();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override int SaveChanges()
        {
            ChangeTracker.SetAuditProperties();
            return base.SaveChanges();
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.SetAuditProperties();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public DbSet<Rent> Rents { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Properties> Properties { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Phone> Phones { get; set; }

        public DbSet<BookCategory> BookCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookCategory>()
                   .HasKey(b => new { b.BookId, b.CategoryId});

            modelBuilder.Entity<Person>()
                   .HasKey(b => new { b.Cpf });

            modelBuilder.Entity<Phone>()
                   .HasKey(d => new { d.PhoneNumber});

            modelBuilder.Entity<Book>()
                .Property(v => v.Price).HasPrecision(7, 2);

            //Maping soft_Delete
            modelBuilder.ApplyConfiguration(new BookMap());
        }
    }
}
