namespace KalikoCMS.Mvc.Framework
{
    using System.Web;
    using System.Web.Routing;

    public class CmsRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection) {
            // Prevent usage of route if not initialized from custom request handler
            return httpContext.Items.Contains("cmsRouting");
        }
    }
}
