using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute]int id)
        {
            _restaurantService.Delete(id);
            return NoContent();
        }

        
        [HttpPut("{id}")]
        public ActionResult Update([FromBody]UpdateRestaurantDto dto, [FromRoute] int id)
        {
            _restaurantService.Update(dto, id);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var id = _restaurantService.Create(dto);
            return Created($"/api/restaurant/{id}", null);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery]RestaurantQuery query)
        {
            var restaurantsDtos = _restaurantService.GetAll(query);
            return Ok(restaurantsDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<RestaurantDto>> Get([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);
            return Ok(restaurant);
        }
    }
}
