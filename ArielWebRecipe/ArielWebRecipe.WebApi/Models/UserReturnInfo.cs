using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArielWebRecipe.WebApi.Models
{
    public class UserReturnInfo
    {
        public string Nickname { get; set; }
        public string SessionKey { get; set; }
    }
}