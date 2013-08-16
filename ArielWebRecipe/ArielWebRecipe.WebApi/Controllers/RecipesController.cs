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
using System.Threading.Tasks;
using System.Text;
using System.Web;
using System.IO;
using ArielWebRecipe.WebApi.Libraries;

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
        public ICollection<RecipeInfo> GetPage(int pageNumber)
        {
            var recipeList = recipeRepository.All().
                OrderByDescending(x => x.Users.Count).Skip(pageNumber * RECIPE_ON_PAGE_COUNT).Take(RECIPE_ON_PAGE_COUNT);
            ICollection<RecipeInfo> recipeInfoList = new List<RecipeInfo>();

            foreach (var recipe in recipeList)
            {
                recipeInfoList.Add(new RecipeInfo
                {
                    Id=recipe.Id,
                    Title = recipe.Title,
                    AuthorName = recipe.Author.Nickname,
                    PictureLink = recipe.PictureLink,
                    PreparationTime = recipe.PreparationSteps.Sum(x => x.PreparationTime),
                    Likes = recipe.Users.Count
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

        [HttpGet]
        [ActionName("search")]
        public IEnumerable<RecipeInfo> Search(string sessionKey)
        {
            var recipies = (from recipes in recipeRepository.All()
                           where recipes.Title.Contains(sessionKey)
                                select recipes).Take(RECIPE_ON_PAGE_COUNT).OrderByDescending(x=>x.Users.Count).Select(RecipeInfo.FromRecipe);

            return recipies;
        }


        [HttpGet]
        [ActionName("upload")]
        async Task<HttpResponseMessage> Upload()
        {
                        string RecipeId = null;
            string SessionKey = null;
            string StepId = null;
            string ImageExtension = null;

            Recipe targetRecipe;

            // Check if the request contains multipart/form-data. 
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data/Uploads");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                StringBuilder sb = new StringBuilder(); // Holds the response body 

                // Read the form data and return an async task. 
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the form data. 
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        if (key == "ImageExtension")
                        {
                            ImageExtension = val;
                        }
                        if (key == "RecipeId")
                        {
                            RecipeId = val;
                        }
                        if (key == "SessionKey")
                        {
                            SessionKey = val;
                        }
                        if (key == "StepId")
                        {
                            StepId = val;
                        }
                        sb.Append(string.Format("{0}: {1}\r\n", key, val));
                    }
                }

                // This illustrates how to get the file names for uploaded files. 
                foreach (var file in provider.FileData)
                {
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);

                    sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n",
                        fileInfo.Name, fileInfo.Length));

                    //string rootFixed = root.Replace("/", "\\");
                    string newName = root + "\\" + SessionKey + RecipeId + StepId + ImageExtension;

                    File.Move(fileInfo.FullName, newName);

                    var uploadedImageURL = DropboxImageUploader.Upload(newName);

                    //deleting temp file 
                    File.Delete(newName);

                    sb.Append("Image uploaded to " + uploadedImageURL);

                    if (RecipeId != null)
                    {
                        int recipeIdInt = int.Parse(RecipeId);

                        targetRecipe = (from recipes in recipeRepository.All()
                                        where recipes.Id== recipeIdInt
                                        select recipes).FirstOrDefault();
                    }
                    else
                    {
                        throw new ApplicationException("Invalid Recipe.");
                    }

                    if (StepId != null)
                    {
                        int StepIdInt = int.Parse(StepId);
                        var step = (from steps in targetRecipe.PreparationSteps
                                    where steps.Id == StepIdInt
                                    select steps).FirstOrDefault();

                        step.PictureLink = uploadedImageURL;
                        stepsRepository.Update(StepIdInt, step);
                    }
                    else
                    {
                        targetRecipe.PictureLink = uploadedImageURL;
                        recipeRepository.Update(targetRecipe.Id, targetRecipe);
                    }
                }

                return new HttpResponseMessage()
                {
                    Content = new StringContent(sb.ToString())
                };
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
