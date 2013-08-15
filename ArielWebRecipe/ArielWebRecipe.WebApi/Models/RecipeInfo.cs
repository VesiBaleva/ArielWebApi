using ArielWebRecipe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ArielWebRecipe.WebApi.Models
{
    public class RecipeInfo
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string PictureLink { get; set; }

        public string AuthorName { get; set; }

        public int PreparationTime { get; set; }

        public int Likes { get; set; }

        public static Expression<Func<Recipe, RecipeInfo>> FromRecipe
        {
            get
            {
                return x => new RecipeInfo 
                { 
                    Id = x.Id, 
                    Title = x.Title, 
                    AuthorName=x.Author.UserName,
                    PictureLink=x.PictureLink, 
                    PreparationTime=x.PreparationSteps.Select(y=>y.PreparationTime).Sum()
                };
            }
        }
    }
}