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
using ArielWebRecipe.WebApi.Models;
using System.IO;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using ArielWebRecipe.WebApi.Libraries;

namespace ArielWebRecipe.WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        private IRepository<User> userRepository;

        public UsersController()
        {
            var dbContext = new RecipeContext();
            this.userRepository = new DbUserRepository(dbContext);
        }

        public UsersController(IRepository<User> repository)
        {
            this.userRepository = repository;
        }

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage LoginUser([FromBody] User user)
        {
            var sessionKey = Guid.NewGuid().ToString();

            var loggedUser = userRepository.All().Where(u => u.UserName == user.UserName && u.Password == user.Password).FirstOrDefault();
            if (loggedUser != null)
            {
                loggedUser.SessionKey = sessionKey;
                userRepository.Update(loggedUser.Id, loggedUser);
                return this.Request.CreateResponse(HttpStatusCode.OK, new UserReturnInfo { Nickname = loggedUser.Nickname, SessionKey = sessionKey } );
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid login!");
        }

        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage RegisterUser([FromBody] User user)
        {
            var sessionKey = Guid.NewGuid().ToString();

            var existingUser = userRepository.All().Where(u => u.UserName == user.UserName).FirstOrDefault();
            if (existingUser == null)
            {
                user.SessionKey = sessionKey;
                userRepository.Add(user);
                return this.Request.CreateResponse(HttpStatusCode.OK, sessionKey);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "User already exists!");
        }

        [HttpGet]
        [ActionName("logout")]
        public void LogoutUser(string sessionKey)
        {
            var loggedUser = userRepository.All().Where(u => u.SessionKey == sessionKey).FirstOrDefault();
            loggedUser.SessionKey = null;
            userRepository.Update(loggedUser.Id, loggedUser);
        }

        [HttpGet]
        [ActionName("test")]
        public IEnumerable<User> Test()
        {
            return userRepository.All();
        }

        [HttpGet]
        [ActionName("checkSessionKey")]
        public void CheckSessionKey(string sessionKey)
        {
            var loggedUser = userRepository.All().Where(u => u.SessionKey == sessionKey).FirstOrDefault();
            if (loggedUser == null)
            {
                throw new ApplicationException("Another Log In required.");
            }
        }

        [HttpPost]
        [ActionName("testUpload")]
        public async Task<HttpResponseMessage> TestUpload()
        {
            string RecipeId = null;
            string SessionKey = null;
            string StepId = null;
            string ImageExtension = null;

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

                    //DropboxImageUploader.Upload(newName);
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
