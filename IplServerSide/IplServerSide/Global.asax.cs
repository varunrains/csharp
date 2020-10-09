using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using IplServerSide.Models;
using Unity.Extension;

namespace IplServerSide
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            //var mapperConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new IplAutomapperProfile());
            //});

            //IMapper mapper = mapperConfig.CreateMapper();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}
