using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Authorization
{
    public class CreateMultipleRequirementHandler : AuthorizationHandler<CreateMultipleRequirement>
    {
        private readonly RestaurantDbContext _context;

        public CreateMultipleRequirementHandler(RestaurantDbContext context)
        {
            _context = context;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreateMultipleRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);
            var countedRestaurants = _context.Restaurants.Count(u => u.CreatedById == userId);
            if (countedRestaurants >= requirement.MinimumRestaurantCreated)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
