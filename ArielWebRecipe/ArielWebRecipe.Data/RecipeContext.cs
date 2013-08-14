using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ArielWebRecipe.Models;

namespace ArielWebRecipe.Data
{
    public class RecipeContext:DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<PreparationStep> PreparationSteps { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>().HasOptional(x => x.Author).WithMany().Map(x=>x.MapKey("AuthorId")).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
