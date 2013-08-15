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

        [HttpPost]
        [ActionName("testUpload")]
        public string TestUpload()
        {
            //var file = File.Create(@"C:\Users\Angel\Documents\GitHub\ArielWebApi\ArielWebRecipe\testResult.txt")


            return Request.Content.ReadAsStringAsync().Result;
        }
    }
}
