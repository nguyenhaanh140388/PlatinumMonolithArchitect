using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

namespace Platinum.WebApiApplication.Rewriter.CustomRules
{
    public class DefaultRedirectRule : IRule
    {
        private readonly string[] matchPaths;
        private readonly PathString newPath;

        public DefaultRedirectRule(string[] matchPaths, string newPath)
        {
            this.matchPaths = matchPaths;
            this.newPath = new PathString(newPath);
        }

        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;

            // if already redirected, skip  
            if (request.Path.StartsWithSegments(new PathString(newPath)))
            {
                return;
            }

            if (matchPaths.Contains(request.Path.Value))
            {
                var newLocation = $"{newPath}{request.QueryString}";

                var response = context.HttpContext.Response;
                response.StatusCode = StatusCodes.Status302Found;
                context.Result = RuleResult.EndResponse;
                response.Headers[HeaderNames.Location] = newLocation;
            }
        }
    }
}
