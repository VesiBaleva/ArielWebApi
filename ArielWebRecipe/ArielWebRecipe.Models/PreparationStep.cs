using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArielWebRecipe.Models
{
    public class PreparationStep
    {
        public int Id { get; set; }

        public string PictureLink { get; set; }

        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public int Order { get; set; }

        public string Description { get; set; }

        public int PreparationTime { get; set; }
    }
}
