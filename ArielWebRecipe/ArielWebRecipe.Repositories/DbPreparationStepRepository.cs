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
    public class DbPreparationStepRepository:IRepository<PreparationStep>
    {
        private DbContext dbContext;
        private DbSet<PreparationStep> entitySet;

        public DbPreparationStepRepository()
        {
            this.dbContext = new RecipeContext();
            this.entitySet = this.dbContext.Set<PreparationStep>();
        }

        public DbPreparationStepRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = this.dbContext.Set<PreparationStep>();
        }

        public PreparationStep Get(int id)
        {
            return this.entitySet.Where(s => s.Id == id).FirstOrDefault();
        }

        public IQueryable<PreparationStep> All()
        {
            return this.entitySet;
        }

        public PreparationStep Add(PreparationStep step)
        {
            this.entitySet.Add(step);
            this.dbContext.SaveChanges();

            return step;
        }

        public PreparationStep Update(int id, PreparationStep step)
        {
            PreparationStep queryStep = this.entitySet.Where(u => u.Id == id).FirstOrDefault();
            if (step.Order != null)
            {
                queryStep.Order = step.Order;
            }
            if (step.PictureLink != null)
            {
                queryStep.PictureLink = step.PictureLink;
            }
            if (step.PreparationTime != null)
            {
                queryStep.PreparationTime = step.PreparationTime;
            }
            if (step.Recipe != null)
            {
                queryStep.Recipe = step.Recipe;
            }

            this.dbContext.SaveChanges();

            return queryStep;
        }

        public void Delete(int id)
        {
            PreparationStep step = this.Get(id);
            this.entitySet.Remove(step);
            this.dbContext.SaveChanges();
        }

        public void Delete(PreparationStep step)
        {
            this.entitySet.Remove(step);
            this.dbContext.SaveChanges();
        }
    }
}
