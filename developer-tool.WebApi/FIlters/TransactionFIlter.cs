using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Core.Interfaces;

namespace WebApi.Filters {
   public class TransactionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            IUnitOfWork unitOfWork = context.HttpContext.RequestServices.GetService<IUnitOfWork>();

            if (context.Exception == null &&
                context.HttpContext.Response.StatusCode >= 200 &&
                context.HttpContext.Response.StatusCode < 300)
            {
                if (context.ModelState.IsValid)
                {
                    unitOfWork.Commit();
                }
            }
        }
    }
}