using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Entities.Exceptions;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        PagedResult<RestaurantDto> GetAll(RestaurantQuery query);
        int Create(CreateRestaurantDto dto);
        void Delete(int id);
        void Update(UpdateRestaurantDto dto, int id);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger, 
            IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public void Update(UpdateRestaurantDto dto, int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
                new ResourceOperationReqiurement(ResourceOperation.Update)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForBidException();
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _logger.LogWarning($"Restaurant with id: {id} DELETE action invoked");
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
                new ResourceOperationReqiurement(ResourceOperation.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForBidException();
            }

            _dbContext.Restaurants.Remove((restaurant));
            _dbContext.SaveChanges();
        }

        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found"); ;

            var result = _mapper.Map<RestaurantDto>(restaurant);
            return result;
        }

        public PagedResult<RestaurantDto> GetAll(RestaurantQuery query)
        {

            var baseQuery = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .Where(x => query.searchPhrase == null || x.Name.ToLower().Contains(query.searchPhrase.ToLower())
                            || x.Description.ToLower().Contains(query.searchPhrase.ToLower()));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectros = new Dictionary<string, Expression<Func<Restaurant, object>>>()
                {
                    {nameof(Restaurant.Name), r => r.Name},
                    {nameof(Restaurant.Description), r => r.Description},
                    {nameof(Restaurant.Category), r => r.Category}
                };
                var selectedColums = columnsSelectros[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC ? baseQuery.OrderBy(selectedColums) : baseQuery.OrderBy(selectedColums);
            }

            var restaurant = baseQuery
                .Skip(query.pageSize*(query.pageNumber-1))
                .Take(query.pageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            var RestaurantDtos = _mapper.Map<List<RestaurantDto>>(restaurant);

            var result = new PagedResult<RestaurantDto>(RestaurantDtos, totalItemsCount, query.pageSize, query.pageNumber);

            return result;
        }

        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContextService.GetUserId;
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();
            return restaurant.Id;
        }
    }
}
