using Platinum.Core.Enums;
using Platinum.Core.Helpers;
using Platinum.Identity.Core.Abstractions.Repositories;
using Platinum.Infrastructure.Services;
using RazorEngine;
using RazorEngine.Templating;
using RazorLight;
using System.Reflection;

namespace Platinum.Identity.Infrastructure.Services
{
    public interface ITemplateService
    {
        Task<string> GetTemplateByCodeAsync<TModel>(EmailTemplateEnum emailTemplateEnum, TModel model) where TModel : class;
    }

    public class TemplateService : ITemplateService
    {
        //private readonly IRazorViewToStringRenderer razorRenderer;
        private readonly IRazorEngineService razorEngineService;
        private readonly IEmailTemplateRepository emailTemplateRepository;

        public TemplateService(
            //IRazorViewToStringRenderer razorRenderer,
            IRazorEngineService razorEngineService,
            IEmailTemplateRepository emailTemplateRepository)
        {
            this.razorEngineService = razorEngineService;
            this.emailTemplateRepository = emailTemplateRepository;
        }

        public async Task<string> GetTemplateByCodeAsync<TModel>(EmailTemplateEnum emailTemplateEnum, TModel model) where TModel : class
        {
            var templateBody = await emailTemplateRepository.GetEntityResultAsync<string>(x => x.TemplateCode == (int)EmailTemplateEnum.ConfirmRegister, p => p.TemplateBody);
            //var templateResult = await razorEngineService.RunCompile(templateHtml, Guid.NewGuid().ToString(), writer, typeof(T), model);
            //return await RazorHelper.TemplateAsync(templateBody, model);

            var assembly = Assembly.Load("Platinum.Identity.Core");

            var engine = new RazorLightEngineBuilder()
           // required to have a default RazorLightProject type,
           // but not required to create a template from string.

           

           .UseEmbeddedResourcesProject(assembly)
           .SetOperatingAssembly(Assembly.Load("Platinum.Identity.Core"))
           .UseMemoryCachingProvider()
           .Build();
            
            // ViewModel model = new ViewModel { Name = "John Doe" };

            string result = await engine.CompileRenderStringAsync(Guid.NewGuid().ToString(), templateBody, model);
            return result;
        }
    }
}
