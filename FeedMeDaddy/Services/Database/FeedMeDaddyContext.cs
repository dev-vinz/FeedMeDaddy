using System;
using FeedMeDaddy.Secure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FeedMeDaddy.Services.Database
{
    public partial class FeedMeDaddyContext : DbContext
    {
        public FeedMeDaddyContext()
        {
            this.Load();
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
        public virtual DbSet<RecipeIngredient> RecipeIngredient { get; set; }
        public virtual DbSet<ShoppingIngredient> ShoppingIngredient { get; set; }
        public virtual DbSet<ShoppingList> ShoppingList { get; set; }
        public virtual DbSet<TypeMenu> TypeMenu { get; set; }
        public virtual DbSet<UnitWeight> UnitWeight { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigDB.ToString());
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
                entity.Property(e => e.LimitDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Ingredient)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ingredien__Categ__09746778");

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.Ingredient)
                    .HasForeignKey(d => d.Unit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ingredient__Unit__0A688BB1");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => new { e.User, e.Date, e.Type })
                    .HasName("PK__Menu__41AAF984AF149775");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.CustomRecipe).IsUnicode(false);

                entity.HasOne(d => d.RecipeNavigation)
                    .WithMany(p => p.Menu)
                    .HasForeignKey(d => d.Recipe)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Menu__Recipe__1E6F845E");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Menu)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Menu__Type__6C190EBB");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Menu)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Menu__User__6B24EA82");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Recipe)
                    .HasForeignKey(d => d.User)
                    .HasConstraintName("FK__Recipe__User__72910220");
            });

            modelBuilder.Entity<RecipeIngredient>(entity =>
            {
                entity.HasKey(e => new { e.RecipeId, e.IngredientId })
                    .HasName("PK__RecipeIn__46336395885EBC33");

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.RecipeIngredient)
                    .HasForeignKey(d => d.IngredientId)
                    .HasConstraintName("FK__RecipeIng__Ingre__078C1F06");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeIngredient)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("FK__RecipeIng__Recip__1F63A897");
            });

            modelBuilder.Entity<ShoppingIngredient>(entity =>
            {
                entity.HasKey(e => new { e.ShoppingId, e.IngredientId })
                    .HasName("PK__Shopping__35D01E3DD53ADDAE");

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.ShoppingIngredient)
                    .HasForeignKey(d => d.IngredientId)
                    .HasConstraintName("FK__ShoppingI__Ingre__0880433F");

                entity.HasOne(d => d.Shopping)
                    .WithMany(p => p.ShoppingIngredient)
                    .HasForeignKey(d => d.ShoppingId)
                    .HasConstraintName("FK__ShoppingI__Shopp__70A8B9AE");
            });

            modelBuilder.Entity<ShoppingList>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.ShoppingList)
                    .HasForeignKey(d => d.User)
                    .HasConstraintName("FK__ShoppingLi__User__6DCC4D03");
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

                entity.Property(e => e.Shortcut)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

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
