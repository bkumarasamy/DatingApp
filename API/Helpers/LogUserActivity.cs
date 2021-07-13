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
             var username=resultContext.HttpContext.User.GetuserName();
            var repo=resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            // var user=await repo.GetUserByIdAsync(userId);
            //var user=await repo.GetUserByNameAsync(username);
            // user.LastActive=DateTime.Now;
            var user=await repo.GetUserByNameAsync((string.IsNullOrEmpty(username)?resultContext.HttpContext.User.GetuserName():username));
            if(user!=null)user.LastActive=DateTime.Now;
            await repo.SaveAllAsync();
        }
    }
}