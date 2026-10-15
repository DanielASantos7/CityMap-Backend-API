using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("/system/api/[controller]")]
    public class scenicSpotController : ControllerBase
    {
        public readonly Model1Context db;

        public scenicSpotController(Model1Context context)
        {
            db = context;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllScenicSpot(
            [FromQuery] int? pageNum,
            [FromQuery] int? pageSize,
            [FromQuery] string? spotName,
            [FromQuery] string? address
            )
        {

            int page = pageNum ?? 1;
            int size = pageSize ?? 10;

            var query = db.ScenicSpots.AsQueryable();

            if (!string.IsNullOrWhiteSpace(spotName))
                query = query.Where(s => s.SpotName.Contains(spotName));

            if (!string.IsNullOrWhiteSpace(address))
                query = query.Where(s => s.Address.Contains(address));

            var total = await query.CountAsync();

            var list = await query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(s => new scenicSpotDTO
                {
                    SpotId = s.SpotId,
                    SpotName = s.SpotName,
                    Address = s.Address,
                    Latitude = s.Latitude.ToString(),
                    Longitude = s.Longitude.ToString(),
                    Phone = s.Phone,
                    Rating = s.Rating.ToString(),
                    TicketPrice = s.TicketPrice,
                    OpeningHours = s.OpeningHours,
                    Description = s.Description,
                    Distance = s.Distance.ToString(),
                    ImageUrl = s.ImageUrl,
                    BusinessType = s.BusinessType
                }).ToListAsync();


            var pageData = new PageData<scenicSpotDTO>(total, list);
            return Ok(new ResponseApi<PageData<scenicSpotDTO>>(200, "success", pageData));
        }

        [HttpGet("{spotId}")]
        public async Task<IActionResult> GetSpot(int spotId)
        {
            if (string.IsNullOrWhiteSpace(spotId.ToString()))
                return BadRequest(new ResponseApi<string>(400, "Esse spot não existe", null));

            var s = await db.ScenicSpots
                .Where(s => s.SpotId == spotId)
                .FirstOrDefaultAsync();

            if (s == null)
                return Ok(new ResponseApi<string>(400, "Esse spot não existe", null));

            var spot = new scenicSpotDTO
            {
                TouristPeriod = s.TouristPeriod,
                SpotId = s.SpotId,
                SpotName = s.SpotName,
                Address = s.Address,
                Latitude = s.Latitude.ToString(),
                Longitude = s.Longitude.ToString(),
                Phone = s.Phone,
                Rating = s.Rating.ToString(),
                TicketPrice = s.TicketPrice,
                OpeningHours = s.OpeningHours,
                Description = s.Description,
                Distance = s.Distance.ToString(),
                ImageUrl = s.ImageUrl,
                BusinessType = s.BusinessType
            };


            return Ok(new ResponseApi<scenicSpotDTO>(200, "success", spot));
        }
    }
}


public class scenicSpotDTO
{
    public string TouristPeriod { get; set; }
    public int SpotId { get; set; }
    public string SpotName { get; set; }
    public string Address { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string Phone { get; set; }
    public string Rating { get; set; }
    public string TicketPrice { get; set; }
    public string OpeningHours { get; set; }
    public string Description { get; set; }
    public string Distance { get; set; }
    public string ImageUrl { get; set; }
    public string BusinessType { get; set; }

}
