using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationReqiurement, Restaurant>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationReqiurement requirement,
            Restaurant restaurant)
        {
            if (requirement._resourceOperation == ResourceOperation.Read ||
                requirement._resourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value;
            if(restaurant.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
