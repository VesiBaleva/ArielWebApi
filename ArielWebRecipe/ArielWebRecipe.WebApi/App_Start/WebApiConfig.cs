using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ArielWebRecipe.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "CookingApi",
                routeTemplate: "api/recipes/recipe/{id}/start/{sessionkey}",
                defaults: new { controller = "cooking", sessionKey = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "UsersApi",
                routeTemplate: "api/users/{action}/{sessionkey}",
                defaults: new { controller = "users", sessionKey = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "RecipesApi",
                routeTemplate: "api/recipes/{action}/{sessionkey}",
                defaults: new { controller = "recipes", sessionKey = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "CommentsApi",
                routeTemplate: "api/recipes/recipe/{id}/comment/{sessionkey}",
                defaults: new { controller = "comments", sessionKey = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "LikesApi",
               routeTemplate: "api/recipes/recipe/{id}/like/{sessionkey}",
               defaults: new { controller = "likes", sessionKey = RouteParameter.Optional }
           );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
