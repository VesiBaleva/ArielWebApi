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
    public class DbUserRepository:IRepository<User>
    {
        private DbContext dbContext;
        private DbSet<User> entitySet;

        public DbUserRepository()
        {
            this.dbContext = new RecipeContext();
            this.entitySet = this.dbContext.Set<User>();
        }

        public DbUserRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = this.dbContext.Set<User>();
        }

        public User Get(int id)
        {
            return this.entitySet.Where(u => u.Id == id).FirstOrDefault();
        }

        public IQueryable<User> All()
        {
            return this.entitySet;
        }

        public User Add(User user)
        {
            this.entitySet.Add(user);
            this.dbContext.SaveChanges();
            return user;
        }

        public User Update(int id, User user)
        {
            User queryUser = this.entitySet.Where(u => u.Id == id).FirstOrDefault();
            if (user.UserName!=null)
            {
                queryUser.UserName = user.UserName;
            }
            if (user.Nickname!=null)
            {
                queryUser.Nickname = user.Nickname;
            }
            if (user.Password!=null)
            {
                queryUser.Password = user.Password;
            }
            if (user.Recipes!=null)
            {
                queryUser.Recipes = user.Recipes;
            }
            if (user.SessionKey!=null)
            {
                queryUser.SessionKey = user.SessionKey;
            }
            this.dbContext.SaveChanges();

            return queryUser;
        }

        public void Delete(int id)
        {
            User queryUser = this.Get(id);
            this.entitySet.Remove(queryUser);
            dbContext.SaveChanges();
        }

        public void Delete(User queryUser)
        {
            this.entitySet.Remove(queryUser);
            dbContext.SaveChanges();
        }
    }
}
