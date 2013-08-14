using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArielWebRecipe.Models
{
    public class Recipe
    {
        public Recipe()
        {
            this.PreparationSteps = new HashSet<PreparationStep>();
            this.Comments = new HashSet<Comment>();
            this.Likes = 0;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string PictureLink { get; set; }

        public int? UserId { get; set; }

        public virtual User User { get; set; }

        public int Likes { get; set; }

        public virtual ICollection<PreparationStep> PreparationSteps { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
