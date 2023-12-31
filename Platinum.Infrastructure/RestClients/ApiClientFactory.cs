﻿using Microsoft.Extensions.Configuration;
using Platinum.Core.Utils;

namespace Platinum.Infrastructure.RestClients
{
    public static class ApiClientFactory
    {
        private static readonly Lazy<ApiClient> restClient = new Lazy<ApiClient>(
        () => new ApiClient(),
        LazyThreadSafetyMode.ExecutionAndPublication);

        public static IConfiguration Configuration => (IConfiguration)ApplicationHttpContext.GetService(typeof(IConfiguration));

        static ApiClientFactory()
        {
            //apiUri = new Uri(ApplicationSettings.WebApiUrl);
        }

        public static ApiClient ORGAInstance
        {
            get
            {
                ApiClient client = restClient.Value;
                client.EAIRESTBaseAddress = new Uri(Configuration["ORGAService:BaseAddress"]);
                client.ServiceUserName = Configuration["ORGAService:UserName"];
                client.ServicePassword = Configuration["ORGAService:Password"];
                return client;
            }
        }
    }
}
