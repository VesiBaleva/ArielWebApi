using ArielWebRecipe.WebApi.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ArielWebRecipe.WebApi.Controllers
{
    public class DropboxController : ApiController
    {
        [HttpPost]
        [ActionName("upload")]
        public void Upload([FromBody] string path)
        {
            DropboxImageUploader.Upload(path);
        }
    }
}
