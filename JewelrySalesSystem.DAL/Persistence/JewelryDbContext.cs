using JewelrySalesSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JewelrySalesSystem.DAL.Persistence
{
    public class JewelryDbContext : DbContext
    {
        public JewelryDbContext()
        {

        }

        public JewelryDbContext(DbContextOptions<JewelryDbContext> options)
            : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=(local);Uid=sa;Pwd=1;Database=JewelrySalesSystem;Trusted_Connection=true;TrustServerCertificate=true;");

        //        //optionsBuilder.UseSqlServer("Server=tcp:jewelrysalessystem.database.windows.net,1433;Initial Catalog=JewelrySalesSystem;Persist Security Info=False;User ID=jss;Password=@Testpassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasOne(c => c.Customer).WithMany(i => i.Invoices)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(c => c.User).WithMany(i => i.Invoices)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(c => c.Warranty).WithMany(i => i.Invoices)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.HasOne(p => p.Product).WithMany(i => i.InvoiceDetails)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(i => i.Invoice).WithMany(i => i.InvoiceDetails)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(b => b.ProductType).WithMany(i => i.Products)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(g => g.Gender).WithMany(i => i.Products)
                    .OnDelete(DeleteBehavior.ClientSetNull);              

                entity.HasOne(c => c.Category).WithMany(i => i.Products)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ProductMaterial>(entity =>
            {
                entity.HasOne(p => p.Product).WithMany(m => m.ProductMaterials)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(m => m.Material).WithMany(o => o.ProductMaterials)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ProductGem>(entity =>
            {
                entity.HasOne(p => p.Product).WithMany(g => g.ProductGems)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(g => g.Gem).WithMany(o => o.ProductGems)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
          
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Carat> Carats { get; set; }
        public DbSet<Cut> Cuts { get; set; }
        public DbSet<Clarity> Clarities { get; set; }
        public DbSet<Shape> Shapes { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductMaterial> ProductMaterials { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialPriceList> MaterialPrices { get; set; }
        public DbSet<Warranty> Warranties { get; set; }
        public DbSet<ProductGem> ProductGems { get; set; }
        public DbSet<Gem> Gems { get; set; }
        public DbSet<GemPriceList> GemPrices { get; set; }
        public DbSet<Counter> Counters { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<CounterType> CounterTypes { get; set; }
    }
}
