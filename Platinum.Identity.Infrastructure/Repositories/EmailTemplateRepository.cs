using Platinum.Core.Abstractions.Identitys;
using Platinum.Identity.Core.Abstractions.Repositories;
using Platinum.Identity.Core.Templates;
using Platinum.Identity.Infrastructure.Persistence;
using Platinum.Infrastructure.Repositories;

namespace Platinum.Identity.Infrastructure.Repositories
{
    public class EmailTemplateRepository : BaseRepository<PlatinumIdentityDbContext, EmailTemplate>, IEmailTemplateRepository
    {

        public EmailTemplateRepository(PlatinumIdentityDbContext clientContext, IApplicationUserManager appUserManager = null) :
            base(clientContext, appUserManager)
        {
        }
    }
}
