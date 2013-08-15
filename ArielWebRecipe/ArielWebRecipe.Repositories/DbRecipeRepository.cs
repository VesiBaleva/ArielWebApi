using ArielWebRecipe.Data;
using ArielWebRecipe.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArielWebRecipe.Repositories
{
    public class DbRecipeRepository : IRepository<Recipe>
    {
        private DbContext dbContext;
        private DbSet<Recipe> entitySet;

        public DbRecipeRepository()
        {
            this.dbContext = new RecipeContext();
            this.entitySet = this.dbContext.Set<Recipe>();
        }

        public DbRecipeRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = this.dbContext.Set<Recipe>();
        }

        public Recipe Get(int id)
        {
            return this.entitySet.Where(r => r.Id == id).FirstOrDefault();
        }

        public IQueryable<Recipe> All()
        {
            return this.entitySet;
        }

        public Recipe Add(Recipe recipe)
        {
            this.entitySet.Add(recipe);
            this.dbContext.SaveChanges();

            return recipe;
        }

        public Recipe Update(int id, Recipe recipe)
        {
            Recipe queryRecipe = this.entitySet.Where(u => u.Id == id).FirstOrDefault();
            if (recipe.Title != null)
            {
                queryRecipe.Title = recipe.Title;
            }
            if (recipe.PictureLink != null)
            {
                queryRecipe.PictureLink = recipe.PictureLink;
            }
            if (recipe.Author != null)
            {
                queryRecipe.Author = recipe.Author;
            }
            if (recipe.Comments != null)
            {
                queryRecipe.Comments = recipe.Comments;
            }
            if (recipe.PreparationSteps!=null)
            {
                queryRecipe.PreparationSteps = recipe.PreparationSteps;
            }

            this.dbContext.SaveChanges();

            return queryRecipe;
        }

        public void Delete(int id)
        {
            Recipe recipe = this.Get(id);
            this.entitySet.Remove(recipe);
            this.dbContext.SaveChanges();
        }

        public void Delete(Recipe recipe)
        {
            this.entitySet.Remove(recipe);
            this.dbContext.SaveChanges();
        }
    }
}
