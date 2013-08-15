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
        public RecipeDetails AddRecipe(string sessionKey, [FromBody] RecipeDetails recipeDetails)
        {
            //creating new Recipe
            var newRecipe = new Recipe();
            newRecipe.Title = recipeDetails.Title;

            //Creating list of preparation steps
            var newSteps = new List<PreparationStep>();
            foreach (var step in recipeDetails.PreparationSteps)
            {
                var newStep = new PreparationStep()
                {
                    Description = step.Description,
                    Order = step.Order,
                    PreparationTime = step.PreparationTime
                };

                newSteps.Add(newStep);
                newRecipe.PreparationSteps.Add(newStep);
            }

            //Check if the user is authentic
            var author = this.userRepository.All().Where(a => a.SessionKey == sessionKey).FirstOrDefault();
            if (author != null)
            {
                newRecipe.Author = author;
                this.recipeRepository.Add(newRecipe);

                //Returning Recipe and Preparation Steps Ids
                recipeDetails.Id = newRecipe.Id;

                int i = 0;
                foreach (var step in recipeDetails.PreparationSteps)
                {
                    step.Id = newSteps[i++].Id;
                }

                return recipeDetails;
            }
            else
            {
                throw new ArgumentOutOfRangeException("You are not an author!!!");
            }
        }

        [HttpGet]
        [ActionName("page")]
        public ICollection<RecipeInfo> GetPage(int sessionKey)
        {
            var recipeList = recipeRepository.All().
                OrderBy(x => x.Users.Count).Skip(sessionKey * RECIPE_ON_PAGE_COUNT).Take(RECIPE_ON_PAGE_COUNT);
            ICollection<RecipeInfo> recipeInfoList = new List<RecipeInfo>();

            foreach (var recipe in recipeList)
            {
                recipeInfoList.Add(new RecipeInfo
                {
                    Id=recipe.Id,
                    Title = recipe.Title,
                    AuthorName = recipe.Author.Nickname,
                    PictureLink = recipe.PictureLink,
                    PreparationTime = recipe.PreparationSteps.Sum(x => x.PreparationTime)
                });
            }

            return recipeInfoList;
        }

        [HttpGet]
        [ActionName("recipe")]
        public RecipeDetails GetRecipe(int sessionKey)
        {
            var recipe = recipeRepository.Get(sessionKey);
            RecipeDetails details = new RecipeDetails
            {
                AuthorName = recipe.Author.Nickname,
                Comments = recipe.Comments.AsQueryable().Select(CommentInfo.FromComment).AsEnumerable(),
                PictureLink = recipe.PictureLink,
                PreparationSteps = recipe.PreparationSteps.AsQueryable().Select(PreparationStepInfo.FromPreparationStep).AsEnumerable(),
                PreparationTime = recipe.PreparationSteps.Sum(x => x.PreparationTime),
                Title = recipe.Title,
                Likes = recipe.Users.Count()
            };

            return details;
        }

        [HttpGet]
        [ActionName("steps")]
        public ICollection<PreparationStepInfo> GetSteps(int sessionKey)
        {
            var steps = recipeRepository.Get(sessionKey).PreparationSteps;
            var stepsInfo = new List<PreparationStepInfo>();

            foreach (var step in steps)
            {
                stepsInfo.Add(new PreparationStepDetails
                {
                    Description = step.Description,
                    Order=step.Order,
                    PictureLink = step.PictureLink,
                    PreparationTime=step.PreparationTime 
                });
            }

            return stepsInfo.OrderBy(x => x.Order).ToList();
        }

        //[HttpGet]
        //[ActionName("page")]
        //public HttpResponseMessage GetRecipe(int sessionKey, int id)
        //{
        //    Recipe recipe = recipeRepository.Get(id);

        //    RecipeDetails fullRecipe=
        //}
    }
}
