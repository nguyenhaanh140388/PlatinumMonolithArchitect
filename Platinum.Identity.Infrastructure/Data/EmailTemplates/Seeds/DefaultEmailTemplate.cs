using Platinum.Core.Enums;
using Platinum.Core.Extensions;
using Platinum.Core.Utils;
using Platinum.Identity.Core.Abstractions.Repositories;
using Platinum.Identity.Core.Templates;
using static Platinum.Core.Common.Constants;

namespace Platinum.Identity.Infrastructure.Data.EmailTemplates.Seeds
{
    public static class DefaultEmailTemplate
    {
        public static async Task SeedAsync(IEmailTemplateRepository emailTemplateRepository)
        {
            var templatesEnum = Enum.GetValues(typeof(EmailTemplateEnum));

            foreach (var template in templatesEnum)
            {
                var templateEnum = (EmailTemplateEnum)template;

                var isExist = emailTemplateRepository.GetQuery(x => !x.IsDeleted && x.TemplateCode == (int)templateEnum).Any();

                if (!isExist)
                {
                    var contents = templateEnum.ToEnumContentDictionary();
                    var emailTemplate = new EmailTemplate()
                    {
                        TemplateName = contents[EmailTemplateProperties.EmailTemplateName],
                        TemplateCode = (int)templateEnum,
                        TemplateBody = ResourceUtils.GetResourceContent(contents[EmailTemplateProperties.FileName],
                        ResourceFolders.ConfirmAccount,
                        ResourceNameSpaces.PlatinumIdentityCoreResource),
                        Subject = contents[EmailTemplateProperties.EmailSubject]
                    };


                    emailTemplateRepository.InsertOrUpdate(emailTemplate);
                }
            }

            await emailTemplateRepository.SaveChangesAsync(null, null);
        }
    }
}
