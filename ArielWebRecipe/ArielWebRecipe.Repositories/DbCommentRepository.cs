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
    public class DbCommentRepository:IRepository<Comment>
    {
        private DbContext dbContext;
        private DbSet<Comment> entitySet;

        public DbCommentRepository()
        {
            this.dbContext = new RecipeContext();
        }

        public DbCommentRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = this.dbContext.Set<Comment>();
        }

        public Comment Get(int id)
        {
            return this.entitySet.Where(c => c.Id == id).FirstOrDefault();
        }

        public IQueryable<Comment> All()
        {
            return this.entitySet;
        }

        public Comment Add(Comment comment)
        {
            this.entitySet.Add(comment);
            this.dbContext.SaveChanges();

            return comment;
        }

        public Comment Update(int id, Comment comment)
        {
            Comment queryComment = this.entitySet.Where(u => u.Id == id).FirstOrDefault();

            if (comment.Content != null)
            {
                queryComment.Content = comment.Content;
            }
            if (comment.Recipe != null)
            {
                queryComment.Recipe = comment.Recipe;
            }
            if (comment.User != null)
            {
                queryComment.User = comment.User;
            }

            this.dbContext.SaveChanges();

            return queryComment;
        }

        public void Delete(int id)
        {
            Comment comment = this.Get(id);
            this.entitySet.Remove(comment);
            this.dbContext.SaveChanges();
        }

        public void Delete(Comment comment)
        {
            this.entitySet.Remove(comment);
            this.dbContext.SaveChanges();
        }
    }
}
