using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArielWebRecipe.WebApi.Models
{
    public class RecipeInfo
    {
        public string Title { get; set; }

        public string PictureLink { get; set; }

        public string AuthorName { get; set; }

        public int Likes { get; set; }

        public int PreparationTime { get; set; }
    }
}