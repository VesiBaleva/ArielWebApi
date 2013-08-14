using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArielWebRecipe.WebApi.Models
{
    public class RecipeDetails : RecipeInfo
    {
        public IEnumerable<PreparationStepInfo> PreparationSteps { get; set; }

        public IEnumerable<CommentInfo> Comments { get; set; }
    }
}