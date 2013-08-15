using ArielWebRecipe.Data;
using ArielWebRecipe.Models;
using ArielWebRecipe.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ArielWebRecipe.WebApi.Controllers
{
    public class LikesController : ApiController
    {
        private IRepository<Recipe> recipeRepository;
        private IRepository<User> userRepository;

        public LikesController()
        {
            var dbContext = new RecipeContext();
            this.recipeRepository = new DbRecipeRepository(dbContext);
            this.userRepository = new DbUserRepository(dbContext);
        }
        
        [HttpGet]
        [ActionName("like")]
        public void Like(int id, string sessionKey)
        {
            var loggedUser = this.userRepository.All().Where(a => a.SessionKey == sessionKey).FirstOrDefault();
            if (loggedUser != null)
            {
                var recipe = recipeRepository.Get(id);
                recipe.Users.Add(loggedUser);
                this.recipeRepository.Update(recipe.Id, recipe);
            }
        }
    }
}
