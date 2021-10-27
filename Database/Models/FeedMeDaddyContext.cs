using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Database.Models
{
    public partial class FeedMeDaddyContext : DbContext
    {
        public FeedMeDaddyContext()
        {
        }

        public FeedMeDaddyContext(DbContextOptions<FeedMeDaddyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FoodCategory> FoodCategory { get; set; }
        public virtual DbSet<Fridge> Fridge { get; set; }
        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<ShoppingList> ShoppingList { get; set; }
        public virtual DbSet<TypeMenu> TypeMenu { get; set; }
        public virtual DbSet<UnitWeight> UnitWeight { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:vinz-dev.database.windows.net,1433;Initial Catalog=FeedMeDaddy;Persist Security Info=False;User ID=dev_vinz;Password=*JrKGXs*VdBEY*LLnP33E5^h&L9@*XJuHKN8Qro5KWni4H#U$G5s9bA$QcUVXNhc74DgBj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodCategory>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Category)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Fridge>(entity =>
            {
                entity.HasKey(e => e.User)
                    .HasName("PK__Fridge__BD20C6F0C97E5C95");

                entity.Property(e => e.User).ValueGeneratedNever();

                entity.Property(e => e.Ingredients)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.UserNavigation)
                    .WithOne(p => p.Fridge)
                    .HasForeignKey<Fridge>(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Fridge__User__6FE99F9F");
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.LimitDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Ingredient)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ingredien__Categ__6477ECF3");

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.Ingredient)
                    .HasForeignKey(d => d.Unit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ingredient__Unit__656C112C");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CustomRecipe).IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.RecipeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Recipe)
                    .HasConstraintName("FK__Menu__Recipe__6D0D32F4");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Menu__Type__6C190EBB");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Menu__User__6B24EA82");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Ingredients)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Recipe)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Recipe__User__68487DD7");
            });

            modelBuilder.Entity<ShoppingList>(entity =>
            {
                entity.HasKey(e => e.User)
                    .HasName("PK__Shopping__BD20C6F0906980DC");

                entity.Property(e => e.User).ValueGeneratedNever();

                entity.Property(e => e.Ingredients)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.UserNavigation)
                    .WithOne(p => p.ShoppingList)
                    .HasForeignKey<ShoppingList>(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShoppingLi__User__72C60C4A");
            });

            modelBuilder.Entity<TypeMenu>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Type)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UnitWeight>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
