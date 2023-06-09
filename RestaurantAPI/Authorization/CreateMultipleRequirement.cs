﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class CreateMultipleRequirement : IAuthorizationRequirement
    {
        public int MinimumRestaurantCreated { get; }
        public CreateMultipleRequirement(int minimumRestaurantCreated)
        {
            MinimumRestaurantCreated = minimumRestaurantCreated;
        }
    }
}
