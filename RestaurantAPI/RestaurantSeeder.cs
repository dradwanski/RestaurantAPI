using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantAPI.Entities;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
            }
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = getRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> getRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                },
            };
            return roles;
        }
        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description = "is an American fast food",
                    ContactEmail = "contact@kfc.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Grander",
                            Price = 10.30M,
                        },
                        new Dish()
                        {
                            Name = "Zinger",
                            Price = 5.30M,
                        }
                    },
                    Address = new Address()
                    {
                        City = "Nakło",
                        Street = "Wierzbowa 4",
                        PostalCode = "42-620"
                    }
                },
                new Restaurant()
                {
                    Name = "McDonald",
                    Category = "Fast Food",
                    Description = "is an American fast food",
                    ContactEmail = "contact@mcd.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "BigMac",
                            Price = 11.30M,
                        },
                        new Dish()
                        {
                            Name = "McRoyal",
                            Price = 6.30M,
                        }
                    },
                    Address = new Address()
                    {
                        City = "Nakło",
                        Street = "Wierzbowa 5",
                        PostalCode = "42-621"
                    }
                }

            };
            return restaurants;
        }
    }
}
