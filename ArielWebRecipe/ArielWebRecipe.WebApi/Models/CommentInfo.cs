using ArielWebRecipe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ArielWebRecipe.WebApi.Models
{
    public class CommentInfo
    {
        public string Content { get; set; }

        public string AuthorName { get; set; }

        public static Expression<Func<Comment, CommentInfo>> FromComment
        {
            get
            {
                return x => new CommentInfo
                {
                    AuthorName = x.User.Nickname,
                    Content = x.Content
                };
            }
        }
    }
}