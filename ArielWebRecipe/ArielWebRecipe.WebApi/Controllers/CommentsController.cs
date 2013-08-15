using ArielWebRecipe.Data;
using ArielWebRecipe.Models;
using ArielWebRecipe.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ArielWebRecipe.WebApi.Controllers
{
    public class CommentsController : ApiController
    {
        private IRepository<User> userRepository;
        private IRepository<Recipe> recipeRepository;
        private IRepository<Comment> commentRepository;

        public CommentsController()
        {
            this.userRepository = new DbUserRepository();
            this.recipeRepository = new DbRecipeRepository();
            this.commentRepository = new DbCommentRepository();
        }

        public CommentsController(IRepository<Recipe> repository)
        {
            this.recipeRepository = repository;
        }

        public void Post(int id, string sessionKey, [FromBody] Comment content)
        {
            var author = this.userRepository.All().Where(a => a.SessionKey == sessionKey).FirstOrDefault();
            if (author != null)
            {
                var recipe = recipeRepository.Get(id);
                var comment = new Comment
                {
                    Content = content.Content,
                    Recipe = recipe,
                    User = author
                };
                commentRepository.Add(comment);
            }
        }
    }
}
