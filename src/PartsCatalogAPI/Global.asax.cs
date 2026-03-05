using System;
using System.Data.Entity;
using System.Web;
using System.Web.Http;
using PartsCatalogAPI.Data;

namespace PartsCatalogAPI
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
            // Initialize database with seed data
            Database.SetInitializer(new PartsCatalogInitializer());
        }
    }
}
