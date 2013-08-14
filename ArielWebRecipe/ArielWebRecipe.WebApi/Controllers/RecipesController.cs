using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ArielWebRecipe.Models;
using ArielWebRecipe.Repositories;
using ArielWebRecipe.Data;
using System.Web.Http.Cors;

namespace ArielWebRecipe.WebApi.Models
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RecipesController : ApiController
    {
        private const int RECIPE_ON_PAGE_COUNT = 9;

        private IRepository<Recipe> recipeRepository;
        private IRepository<User> userRepository;

        public RecipesController()
        {
            var dbContext = new RecipeContext();
            this.recipeRepository = new DbRecipeRepository(dbContext);
            this.userRepository = new DbUserRepository(dbContext);
        }

        public RecipesController(IRepository<Recipe> repository)
        {
            this.recipeRepository = repository;
        }

        [HttpPost]
        [ActionName("add")]
        public void AddRecipe(string sessionKey, [FromBody] Recipe recipe)
        {
            var author = this.userRepository.All().Where(a => a.SessionKey == sessionKey).FirstOrDefault();
            if (author != null)
            {
                recipe.Author = author;
                this.recipeRepository.Add(recipe);
            }
        }

        [HttpGet]
        [ActionName("page")]
        public ICollection<RecipeInfo> GetPage(int sessionKey)
        {
            var recipeList = recipeRepository.All().OrderBy(x => x.Users.Count).Skip(sessionKey * RECIPE_ON_PAGE_COUNT).Take(RECIPE_ON_PAGE_COUNT);
            ICollection<RecipeInfo> recipeInfoList = new List<RecipeInfo>();

            foreach (var recipe in recipeList)
            {
                recipeInfoList.Add(new RecipeInfo
                {
                    Title = recipe.Title,
                    AuthorName = recipe.Author.Nickname,
                    PictureLink = recipe.PictureLink,
                    PreparationTime = recipe.PreparationSteps.Sum(x => x.PreparationTime)
                });
            }

            return recipeInfoList;
        }
    }
}
