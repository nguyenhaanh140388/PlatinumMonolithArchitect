using Platinum.Infrastructure.Data.EntityFramework;

namespace Platinum.Infrastructure.Repositories
{
    public class EmailTemplateRepository : BaseRepository<Anhny010920AdministratorContext, EmailTemplate>, IEmailTemplateRepository
    {

        public EmailTemplateRepository(Anhny010920AdministratorContext clientContext, IAppUserManager appUserManager = null) :
            base(clientContext, appUserManager)
        {
        }
    }
}
