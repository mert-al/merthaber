using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HaberSitesi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             name: "AlbumDetails",
             url: "galeri/{categoryUrl}/{albumUrl}",
             defaults: new { controller = "Album", action = "Details" },
             namespaces: new string[] { "HaberSitesi.Controllers" }
           );
           routes.MapRoute(
              name: "Hata",
              url: "hata/{kod}",
              defaults: new { controller = "Error", action = "Page404", kod = UrlParameter.Optional },
              namespaces: new string[] { "HaberSitesi.Controllers" }
          );
            routes.MapRoute(
             name: "AlbumDetailsImageIndex",
             url: "galeri/{categoryUrl}/{albumUrl}/{imageId}",
             defaults: new { controller = "Album", action = "Details" },
             namespaces: new string[] { "HaberSitesi.Controllers" }
           ); 
            routes.MapRoute(
             name: "Reviews",
             url: "Reviews/Create",
             defaults: new { controller = "Reviews", action = "Create" },
             namespaces: new string[] { "HaberSitesi.Controllers" }
           );routes.MapRoute(
             name: "Search",
             url: "Search/Index",
             defaults: new { controller = "Search", action = "Index" },
             namespaces: new string[] { "HaberSitesi.Controllers" }
           );

            routes.MapRoute(
             name: "Albums",
             url: "galeri/{categoryUrl}",
             defaults: new { controller = "Category", action = "Albums" },
             namespaces: new string[] { "HaberSitesi.Controllers" }
           );

            routes.MapRoute(
            name: "AlbumHome",
            url: "galeri",
            defaults: new { controller = "Album", action = "Index" },
            namespaces: new string[] { "HaberSitesi.Controllers" }
          );
            routes.MapRoute(
            name: "VideoHome",
            url: "video",
            defaults: new { controller = "Video", action = "Index" },
            namespaces: new string[] { "HaberSitesi.Controllers" }
      );
            routes.MapRoute(
            name: "Video",
            url: "video/{categoryUrl}",
            defaults: new { controller = "Category", action = "Video" },
            namespaces: new string[] { "HaberSitesi.Controllers" }
          );
            routes.MapRoute(
            name: "VideoDetails",
            url: "video/{categoryUrl}/{videoUrl}",
            defaults: new { controller = "Video", action = "Details" },
            namespaces: new string[] { "HaberSitesi.Controllers" }
          );


            routes.MapRoute(
             name: "Contact",
             url: "iletişim",
             defaults: new { controller = "ContactForm", action = "Index" },
             namespaces: new string[] { "HaberSitesi.Controllers" }
           );
            routes.MapRoute(
              name: "NewsHome",
              url: "",
              defaults: new { controller = "Home", action = "Index" },
              namespaces: new string[] { "HaberSitesi.Controllers" }
            );

            routes.MapRoute(
             name: "Category",
             url: "{categoryUrl}",
             defaults: new { controller = "Category", action = "News" },
             namespaces: new string[] { "HaberSitesi.Controllers" }
           );

            routes.MapRoute(
              name: "Details",
              url: "{categoryUrl}/{newsUrl}",
              defaults: new { controller = "News", action = "Details" },
              namespaces: new string[] { "HaberSitesi.Controllers" }
            );




            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "HaberSitesi.Controllers" }
            );
        }
    }
}
