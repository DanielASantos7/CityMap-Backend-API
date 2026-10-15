using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("system/api/[controller]")]
    public class restaurantController : ControllerBase
    {
        public readonly Model1Context db;

        public restaurantController(Model1Context context)
        {
            db = context;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllRestaurants(
            [FromQuery] int? pageNum,
            [FromQuery] int? pageSize,
            [FromQuery] string? restaurantName,
            [FromQuery] string? restaurantType
        )
        {
            int page = pageNum ?? 1;
            int size = pageSize ?? 10;

            var query = db.Restaurants.AsQueryable();

            if (!string.IsNullOrWhiteSpace(restaurantName))
                query = query.Where(r => r.RestaurantName.Contains(restaurantName));

            if (!string.IsNullOrWhiteSpace(restaurantType))
                query = query.Where(r => r.RestaurantType.Contains(restaurantType));

            int total = await query.CountAsync();

            var lista = await query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(r => new RestauranteDto
                {
                    RestaurantId = r.RestaurantId,
                    RestaurantName = r.RestaurantName,
                    Address = r.Address,
                    Phone = r.Phone,
                    Rating = r.Rating.ToString(),
                    PriceRange = r.PriceRange,
                    RestauranteType = r.RestaurantType,
                    ImageUrl = r.ImageUrl,
                    Description = r.Description,
                    Distance = r.Distance.ToString(),
                    BusinessType = r.BusinessType,
                })
                .ToListAsync();

            if (lista.Count <= 0)
            {
                return Ok(new ResponseApi<string>(200, "Não há restaurantes", null));
            }

            var pageData = new PageData<RestauranteDto>(total, lista);
            return Ok(new ResponseApi<PageData<RestauranteDto>>(200, "success", pageData));
        }


        [HttpGet("restaurantId")]
        public async Task<IActionResult> GetRestaurante(int id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
                return BadRequest(new ResponseApi<string>(400, "Esse restaurante não existe", null));

            var restaurante = await db.Restaurants
                .Where(r => r.RestaurantId == id)
                .Select(r => new RestauranteDto
                {
                    RestaurantId = r.RestaurantId,
                    RestaurantName = r.RestaurantName,
                    Address = r.Address,
                    Phone = r.Phone,
                    Rating = r.Rating.ToString(),
                    PriceRange = r.PriceRange,
                    RestauranteType = r.RestaurantType,
                    ImageUrl = r.ImageUrl,
                    Description = r.Description,
                    Distance = r.Distance.ToString(),
                    BusinessType = r.BusinessType,
                })
                .FirstOrDefaultAsync();

            if (restaurante == null)
                return NotFound(new ResponseApi<string>(404, "Não foi possivel encontrar esse restaurante", null));


            return Ok(new ResponseApi<RestauranteDto>(200, "sucess", restaurante));
        }


    }
}

public record ResponseApi<T>(
    int code,
    string msg,
    T? data
    );

public record PageData<T>(
    int Total,
    IEnumerable<T> Rows
    );

public class RestauranteDto
{
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Rating { get; set; }
    public string PriceRange { get; set; }
    public string RestauranteType { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public string Distance { get; set; }
    public string BusinessType { get; set; }
}