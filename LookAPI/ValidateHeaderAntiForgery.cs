using GenHelper;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;


namespace WebAPI
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)] 
    public sealed class ValidateHeaderAntiForgery : ActionFilterAttribute
    {
        MemoryCacher cacher = new MemoryCacher();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var allowedMMetode = new[] { "api/LockDoor/RegisterLogin", "api/LockDoor/OpenDoor", "api/LockDoor/GetJalanSetapak", "api/LockDoor/GetDoorChecking" };
            StringValues headerValues = "";
            var userId = string.Empty;
            string currentTemplate = filterContext.ActionDescriptor.AttributeRouteInfo.Template;
            string checker = string.Empty;
            string TheSun = string.Empty;
            if (allowedMMetode.Contains(currentTemplate))
            {
                //do something here
               // string akuboleh = "";
            }
            else
            {
                if (filterContext.HttpContext.Request.Headers.TryGetValue("TheSun", out headerValues))
                {
                    TheSun = headerValues.FirstOrDefault();
                }
                if (TheSun == "TheSun")
                {
                    throw new ArgumentNullException("filterContext");
                }
                else
                {
                    if (TheSun.Contains("TheSun"))
                    {
                        checker = TheSun.Replace("TheSun", string.Empty);
                        var checkerResult = cacher.GetValue(checker);
                        if (checkerResult == null)
                        {
                            throw new ArgumentNullException("filterContext");

                        }
                        else
                        {

                        }

                    }
                    else
                    {
                        throw new ArgumentNullException("filterContext");

                    }
                }
            }


            //string a = string.Empty;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string a = string.Empty;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            string a = string.Empty;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            string a = string.Empty;
        }
        //public void OnAuthorization(AuthorizationContext filterContext)
        //{ 
        //    if (filterContext == null) 
        //    { 
        //        throw new ArgumentNullException("filterContext"); 
        //    } 


        //    var httpContext = filterContext.HttpContext; 
        //    var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName]; 
        //    IAntiforgery.Validate(cookie != null ? cookie.Value : null, httpContext.Request.Headers["__RequestVerificationToken"]); 
        //} 
        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    if (context != null)
        //    {
        //       // throw new ArgumentNullException("filterContext");
        //    }
        //}
        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    if (context != null)
        //    {
        //        // throw new ArgumentNullException("filterContext");
        //    }
        //}

        //public override void OnActionExecuted(ActionExecutedContext context) { }

        //public override void OnActionExecuting(ActionExecutingContext context) { }

        //public override void OnResultExecuted(ResultExecutedContext context) { }
        ////
        //public override void OnResultExecuting(ResultExecutingContext context) { }
        //public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) { }
        //public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next) { }

    }

}
