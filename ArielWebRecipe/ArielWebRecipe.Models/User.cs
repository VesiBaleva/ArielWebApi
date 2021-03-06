﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArielWebRecipe.Models
{
    public class User
    {
        public User()
        {
            this.Recipes = new HashSet<Recipe>();
        }

        public int Id { get; set; }

        public string UserName { get; set; }

        public string  Password { get; set; }

        public string Nickname { get; set; }

        public string SessionKey { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
