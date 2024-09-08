using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LeVaTiShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            /*routes.MapRoute(
                name: "Facebook",
                url: "signin-facebook",
                defaults: new { controller = "Account", action = "ExternalLoginCallback" }
            );
            routes.MapRoute(
                name: "OrderSuccess",
                url: "OrderSuccess",
                defaults: new { controller = "Cart", action = "OrderConfirmation", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Checkout",
                url: "Checkout",
                defaults: new { controller = "Cart", action = "Checkout", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Cart",
                url: "Cart",
                defaults: new { controller = "Cart", action = "Cart", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Help",
                url: "Help",
                defaults: new { controller = "Home", action = "Help", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Register",
                url: "Register",
                defaults: new { controller = "Login", action = "Register", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Login", action = "Login", url = UrlParameter.Optional }
            );*/
            routes.MapRoute(
                name: "Brand",
                url: "trang-{nameBrand}",
                defaults: new { controller = "Home", action = "Search", nameBrand = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
