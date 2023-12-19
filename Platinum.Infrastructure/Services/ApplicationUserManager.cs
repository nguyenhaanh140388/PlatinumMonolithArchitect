using System.Security.Claims;

namespace Platinum.Infrastructure.Services
{

    public class ApplicationUserManager : IApplicationUserManager
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IMapper mapper;

        public ApplicationUserManager(SignInManager<ApplicationUser> signInManager,
            IActionContextAccessor actionContextAccessor,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.signInManager = signInManager;
            this.actionContextAccessor = actionContextAccessor;
        }

        public ClaimsPrincipal CurrentUser
        {
            get
            {
                return actionContextAccessor?.ActionContext?.HttpContext?.User;
            }
        }

        public UserManager<ApplicationUser> UserManager
        {
            get
            {
                return signInManager?.UserManager;
            }
        }

        public async Task<ApplicationUser> GetApplicationUser()
        {
            return await UserManager.GetUserAsync(CurrentUser);
        }
    }
}
