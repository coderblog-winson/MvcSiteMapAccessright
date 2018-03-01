using MvcSiteMapAccessright.DI.StructureMap.Registries;
using MvcSiteMapProvider.Loader;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Xml;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcSiteMapAccessright
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Create the DI container (typically part of your DI setup already)
            var container = new Container();
            // Setup configuration of DI (required)
            container.Configure(r => r.AddRegistry<MvcSiteMapProviderRegistry>());
            // Setup global sitemap loader (required)
            MvcSiteMapProvider.SiteMaps.Loader = container.GetInstance<ISiteMapLoader>();
            // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider (optional)
            var validator = container.GetInstance<ISiteMapXmlValidator>();
            validator.ValidateXml(HostingEnvironment.MapPath("~/Mvc.sitemap"));
            // Register the Sitemaps routes for search engines (optional)
            XmlSiteMapController.RegisterRoutes(RouteTable.Routes); 
        }
    }
}
