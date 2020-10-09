using System.Web.Http;
using AutoMapper;
using IplServerSide.Core.Repositories;
using IplServerSide.Models;
using IplServerSide.Persistence;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace IplServerSide
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new IplAutomapperProfile());
            });

            container.RegisterInstance<IMapper>(mapperConfig.CreateMapper(), new ContainerControlledLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}