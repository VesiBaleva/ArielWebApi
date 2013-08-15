using ArielWebRecipe.Data;
using ArielWebRecipe.Models;
using ArielWebRecipe.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using ArielWebRecipe.WebApi.Libraries;

namespace ArielWebRecipe.WebApi.Controllers
{
    public class CookingController : ApiController
    {
        private IRepository<Recipe> recipeRepository;
        private IRepository<User> userRepository;

        public CookingController()
        {
            var dbContext = new RecipeContext();
            this.recipeRepository = new DbRecipeRepository(dbContext);
            this.userRepository = new DbUserRepository(dbContext);
        }

        [HttpGet]
        [ActionName("start")]
        public void StartCooking(int id, string sessionKey)
        { 
            List<PreparationStep> steps = recipeRepository.Get(id).PreparationSteps.OrderBy(x => x.Order).ToList();
            string channel = userRepository.All().Where(x => x.SessionKey == sessionKey).FirstOrDefault().Nickname;

            Thread t = new Thread(new ThreadStart(() => PubnubPublisher.PubnubThread(steps, channel, sessionKey)));
            t.Start();
        }
    }
}
