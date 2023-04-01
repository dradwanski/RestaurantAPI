using System;

namespace RestaurantAPI.Entities.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }

    }
}