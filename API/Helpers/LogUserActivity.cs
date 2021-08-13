using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext=await next();
            if(resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            // var userId=resultContext.HttpContext.User.GetuserId();
             var username=resultContext.HttpContext.User.GetUsername();
            var uow=resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
            // var user=await repo.GetUserByIdAsync(userId);
            //var user=await repo.GetUserByNameAsync(username);
            // user.LastActive=DateTime.Now;
            var user=await uow.UserRepository.GetUserByNameAsync((string.IsNullOrEmpty(username)?resultContext.HttpContext.User.GetUsername():username));
            if(user!=null)user.LastActive=DateTime.UtcNow;
            await uow.Complete();
        }
    }
}