using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using BoxOffice.Api.Bootstrapping.Modules;
using BoxOffice.DAL;
using BoxOffice.DAL.Interfaces;
using System.Web.Http;
using System.Web.Mvc;

namespace BoxOffice.Api
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            //// Register dependencies in controllers
            ////builder.RegisterControllers(typeof(WebApiApplication).Assembly);
            //builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);

            //// Register dependencies in filter attributes
            //builder.RegisterFilterProvider();

            //// Register dependencies in custom views    
            //builder.RegisterSource(new ViewRegistrationSource());
            
            //// Register our Data dependencies
            //builder.RegisterModule(new AutoMapperModule());

            //var container = builder.Build();

            //// Set MVC DI resolver to use our Autofac container
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            // register controllers
            builder.RegisterControllers(typeof (WebApiApplication).Assembly);

            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);

            // Register our Data dependencies
            builder.RegisterModule(new AutoMapperModule());
 

            // build container
            IContainer container = builder.Build();

            // MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // webapi
            GlobalConfiguration.Configuration.DependencyResolver =
                new AutofacWebApiDependencyResolver(container);
        }
    }
}