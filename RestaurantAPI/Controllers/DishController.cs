using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Collections.Generic;

namespace RestaurantAPI.Controllers
{

    [Route("/api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }
        
        [HttpPost]
        public ActionResult Post([FromRoute]int restaurantId, [FromBody]CreateDishDto dto)
        {
            var newDishId = _dishService.Create(restaurantId, dto);

            return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDto> GetById([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            DishDto DishById = _dishService.GetById(restaurantId, dishId);
            return Ok(DishById);
        }
        [HttpGet]
        public ActionResult<List<DishDto>> Get([FromRoute] int restaurantId)
        {
            var Dishes = _dishService.GetAll(restaurantId);
            return Ok(Dishes);

        }

        [HttpDelete]
        public ActionResult Remove([FromRoute] int restaurantId)
        {
            _dishService.remove(restaurantId);
            return NoContent();
        }

        [HttpDelete("{dishId}")]
        public ActionResult RemoveById([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            _dishService.removeById(restaurantId, dishId);
            return NoContent();
        }
    }
}
