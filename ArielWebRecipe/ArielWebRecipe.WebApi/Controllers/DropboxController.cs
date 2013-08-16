using ArielWebRecipe.Data;
using ArielWebRecipe.Models;
using ArielWebRecipe.Repositories;
using ArielWebRecipe.WebApi.Libraries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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

                    sb.Append("Image uploaded to " + uploadedImageURL);

                    if (RecipeId != null)
                    {
                        targetRecipe = (from recipes in recipeRepository.All()
                                        where recipes.Id == int.Parse(RecipeId)
                                        select recipes).FirstOrDefault();
                    }
                    else
                    {
                        throw new ApplicationException("Invalid Recipe.");
                    }

                    if (StepId != null)
                    {
                        var step = (from steps in targetRecipe.PreparationSteps
                                    where steps.Id == int.Parse(StepId)
                                    select steps).FirstOrDefault();

                        step.PictureLink = uploadedImageURL;
                    }
                    else
                    {
                        targetRecipe.PictureLink = uploadedImageURL;
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
