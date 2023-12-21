using Hangfire.Dashboard;

namespace Platinum.WebApiApplication.Filters.HangFires
{
    public class HangireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string[] _roles;

        public HangireAuthorizationFilter(params string[] roles)
        {
            _roles = roles;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = ((AspNetCoreDashboardContext)context).HttpContext;
            var result = _roles.Aggregate(false, (current, role) => current || httpContext.User.IsInRole(role));

            return result;
        }
    }
}
