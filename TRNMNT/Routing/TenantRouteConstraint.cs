using Microsoft.AspNetCore.Routing;
using System.Linq;
using Microsoft.AspNetCore.Http;
using TRNMNT.Core.Services;

namespace TRNMNT.Web.Routing
{
    public class TenantRouteConstraint : IRouteConstraint
    {
        private IEventService eventService;

        public TenantRouteConstraint(IEventService eventService)
        {
            this.eventService = eventService;
        }


        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey("homepage"))
            {
                values.Add("homepage", "event-info");
            }

            return true;    
        


        var fullAddress = httpContext.Request.Headers["Host"].FirstOrDefault().Split('.');
            if (fullAddress.Length < 2)
            {
                return false;
            }

            var tenantSubdomain = fullAddress[0];
            var eventId = eventService.GetEventIdAsync(tenantSubdomain).Result;

        if (!values.ContainsKey("eventId"))
            {
                values.Add("eventId", eventId);
            }

            return true;
        }
    }
}

