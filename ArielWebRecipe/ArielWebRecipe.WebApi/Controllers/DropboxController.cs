using ArielWebRecipe.Data;
using ArielWebRecipe.Models;
using ArielWebRecipe.Repositories;
using ArielWebRecipe.WebApi.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ArielWebRecipe.WebApi.Controllers
{
    public class DropboxController : ApiController
    {
        private IRepository<Recipe> recipeRepository;
        private IRepository<User> userRepository;

        public DropboxController()
        {
            var dbContext = new RecipeContext();
            this.recipeRepository = new DbRecipeRepository(dbContext);
            this.userRepository = new DbUserRepository(dbContext);
        }

        [HttpPost]
        [ActionName("upload")]
        public void Upload(int id, string sessionKey, [FromBody] string path)
        {
            var user = userRepository.All().Where(x => x.SessionKey == sessionKey).FirstOrDefault();
            if (user != null)
            {
                string link = DropboxImageUploader.Upload(path);
                var recipe = this.recipeRepository.Get(id);
                recipe.PictureLink = link;
                this.recipeRepository.Update(recipe.Id, recipe);
            }
        }
    }
}
