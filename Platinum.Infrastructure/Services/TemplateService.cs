namespace Platinum.Infrastructure.Services
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
            //string template = "/Views/HelloWorld.cshtml";
            //var aaa = await razorRenderer.RenderViewToStringAsync(template, payload);
            this.razorEngineService = razorEngineService;
            this.emailTemplateRepository = emailTemplateRepository;
        }

        public async Task<string> GetTemplateByCodeAsync<TModel>(EmailTemplateEnum emailTemplateEnum, TModel model) where TModel : class
        {
            var templateBody = await emailTemplateRepository.GetEntityResultAsync<string>(x => x.TemplateCode == (int)EmailTemplateEnum.ConfirmRegister, p => p.TemplateBody);
            var templateResult = await RazorHelper.TemplateAsync(templateBody, model);
            return templateResult;
        }
    }
}
