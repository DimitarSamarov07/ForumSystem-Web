namespace ForumSystem.Web.Infrastructure.Attributes
{
    using System.Linq;
    using System.Reflection;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class AuthAttribute : ActionFilterAttribute
    {
        private readonly string _role;

        public AuthAttribute(string role)
        {
            this._role = role;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (this.SkipAuthorization(filterContext))
            {
                if (!filterContext.HttpContext.User.IsInRole(this._role))
                {
                    filterContext.Result = new RedirectToActionResult("401", "Error", null);
                }
                else
                {
                    base.OnResultExecuting(filterContext);
                }
            }

            base.OnResultExecuting(filterContext);
        }

        private bool SkipAuthorization(ResultExecutingContext filterContext)
        {
            var attribute = (filterContext.ActionDescriptor as ControllerActionDescriptor)
                .MethodInfo
                .GetCustomAttributes<AllowAnonymousAttribute>().FirstOrDefault();

            return attribute == null;
        }
    }
}
