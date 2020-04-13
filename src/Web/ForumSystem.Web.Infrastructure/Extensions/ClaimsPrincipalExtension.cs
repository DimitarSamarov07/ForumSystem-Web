namespace ForumSystem.Web.Infrastructure.Extensions
{
    using System;
    using System.Security.Principal;
    using Common;

    public static class ClaimsPrincipalExtension
    {
        public static bool IsAdmin(this IPrincipal principal)
        {
            try
            {
                return principal?.IsInRole(GlobalConstants.AdministratorRoleName) == true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
