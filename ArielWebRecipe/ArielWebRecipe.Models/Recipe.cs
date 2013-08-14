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
            this.Users = new HashSet<User>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string PictureLink { get; set; }

        public virtual User Author{ get; set; }

        public virtual ICollection<PreparationStep> PreparationSteps { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
