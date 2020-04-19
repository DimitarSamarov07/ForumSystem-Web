namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using ForumSystem.Common;
    using ForumSystem.Web.Controllers;
    using Infrastructure.Attributes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Auth(GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
