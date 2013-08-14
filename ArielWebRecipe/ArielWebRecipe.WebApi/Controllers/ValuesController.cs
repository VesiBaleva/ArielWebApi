using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ArielWebRecipe.Data;
using ArielWebRecipe.Models;

namespace ArielWebRecipe.WebApi.Controllers
{
    public class UsersController : ApiController
    {
        RecipeContext context = new RecipeContext();

        // GET api/values
        public IEnumerable<User> Get()
        {
            var users = context.Users.ToList();
            return users;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}