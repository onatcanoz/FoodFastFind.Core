using AppCore.DataAccess.Configs;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.EntityFrameWork.Context
{
    public class FoodFastFindContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<FoodMaterial> FoodMaterials { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Material> Materials { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionConfig.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Food>()
                .HasOne(food => food.Category)
                .WithMany(category => category.Foods)
                .HasForeignKey(food => food.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FoodMaterial>()
                .HasOne(f => f.Food)
                .WithMany(fm => fm.FoodMaterials)
                .HasForeignKey(fi => fi.FoodId);

            modelBuilder.Entity<FoodMaterial>()
                .HasOne(m => m.Material)
                .WithMany(fm => fm.FoodMaterials)
                .HasForeignKey(mi => mi.MaterialId)
                .OnDelete(DeleteBehavior.NoAction);

        }


    }
}
