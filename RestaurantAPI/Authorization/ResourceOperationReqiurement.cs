using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public enum ResourceOperation
    {
        Create,
        Read,
        Update,
        Delete
    }
    public class ResourceOperationReqiurement : IAuthorizationRequirement
    {
        public ResourceOperation _resourceOperation { get; }

        public ResourceOperationReqiurement(ResourceOperation resourceOperation)
        {
            _resourceOperation = resourceOperation;
        }
    }
}
