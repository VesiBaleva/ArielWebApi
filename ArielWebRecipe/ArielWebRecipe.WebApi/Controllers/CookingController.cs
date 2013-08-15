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

            Thread t = new Thread(new ThreadStart(() => PubnubThread(steps, sessionKey)));
            t.Start();
        }

        public void PubnubThread(List<PreparationStep> steps, string sessionKey)
        {
            PubnubAPI pubnub = new PubnubAPI(
                    "pub-c-26f81a7d-18b7-4472-976d-f6d6ba477ee0",               // PUBLISH_KEY
                    "sub-c-8dc89202-0580-11e3-8dc9-02ee2ddab7fe",               // SUBSCRIBE_KEY
                    "sec-c-ZWMwYzA1N2MtNTRkYy00ZjhkLTg0NGItNTdmMDJhNDA5MWY3",   // SECRET_KEY
                    true);

            foreach (var step in steps)
            {
                Thread.Sleep(step.PreparationTime * 1000);
                string channel = userRepository.All().Where(x => x.SessionKey == sessionKey).FirstOrDefault().Nickname;
                string message = "Step " + step.Order + " Completed";

                pubnub.Publish(channel, message);
            }
        }
    }
}
