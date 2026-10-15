using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("system/api/[controller]")]
    public class ReviewController : ControllerBase
    {
        public readonly Model1Context db;

        public ReviewController(Model1Context context)
        {
            db = context;
        }

        // 1. LISTA PÚBLICA (GET system/api/review/list)[cite: 19]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllReviews(
            [FromHeader] string? tokenHeader,
            [FromQuery] int? pageNum,
            [FromQuery] int? pageSize,
            [FromQuery] string? businessType,
            [FromQuery] int? businessId
        )
        {
            if (!UsuarioAtual.IsValido(tokenHeader))
                return Ok(new ResponseApi<string>(401, "Token de autorização inválido ou expirado", null));

            int page = pageNum ?? 1, size = pageSize ?? 10;
            var query = db.Reviews.AsQueryable();

            if (!string.IsNullOrWhiteSpace(businessType))
                query = query.Where(b => b.BusinessType == businessType);

            if (businessId > 0)
                query = query.Where(b => b.BusinessId == businessId);

            int total = await query.CountAsync();

            var list = await query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(b => new ReviewDTO
                {
                    ReviewId = b.ReviewId,
                    UserId = b.UserId,
                    UserName = b.User.UserName,
                    BusinessType = b.BusinessType,
                    BusinessId = b.BusinessId,
                    Rating = b.Rating.ToString(),
                    ReviewContent = b.ReviewContent,
                    ReviewTime = b.ReviewTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    ImageUrls = b.ReviewImages.Select(img => img.ImageUrl).ToList(),
                    LikeCount = b.LikeCount,
                })
                .ToListAsync();

            return Ok(new ResponseApi<PageData<ReviewDTO>>(200, "success", new PageData<ReviewDTO>(total, list)));
        }

        // 2. MINHAS AVALIAÇÕES (GET system/api/review/mine)[cite: 20]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyReviews(
            [FromHeader] string? tokenHeader,
            [FromQuery] int? pageNum,
            [FromQuery] int? pageSize,
            [FromQuery] string? businessType,
            [FromQuery] int? businessId
        )
        {
            if (!UsuarioAtual.IsValido(tokenHeader))
                return Ok(new ResponseApi<string>(401, "Token de autorização inválido ou expirado", null));

            int page = pageNum ?? 1, size = pageSize ?? 10;
            var query = db.Reviews.Where(r => r.UserId == UsuarioAtual.UserId);

            if (!string.IsNullOrWhiteSpace(businessType))
                query = query.Where(b => b.BusinessType == businessType);

            if (businessId > 0)
                query = query.Where(b => b.BusinessId == businessId);

            int total = await query.CountAsync();

            var list = await query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(b => new ReviewDTO
                {
                    ReviewId = b.ReviewId,
                    UserId = b.UserId,
                    UserName = b.User.UserName,
                    BusinessType = b.BusinessType,
                    BusinessId = b.BusinessId,
                    Rating = b.Rating.ToString(),
                    ReviewContent = b.ReviewContent,
                    ReviewTime = b.ReviewTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    ImageUrls = b.ReviewImages.Select(img => img.ImageUrl).ToList(),
                    LikeCount = b.LikeCount,
                })
                .ToListAsync();

            return Ok(new ResponseApi<PageData<ReviewDTO>>(200, "success", new PageData<ReviewDTO>(total, list)));
        }

        // 3. ADICIONAR (POST system/api/review/add)[cite: 21]
        [HttpPost("add")]
        public async Task<IActionResult> AddReview(
            [FromHeader] string? tokenHeader,
            [FromBody] AddReviewDTO dto
        )
        {
            if (!UsuarioAtual.IsValido(tokenHeader))
                return Ok(new ResponseApi<string>(401, "Token de autorização inválido ou expirado", null));

            if (string.IsNullOrWhiteSpace(dto.BusinessType) || dto.BusinessId <= 0 || string.IsNullOrWhiteSpace(dto.ReviewContent))
                return Ok(new ResponseApi<string>(400, "Preencha os dados corretamente", null));

            // Salva a avaliação
            var review = new Review
            {
                UserId = (int)UsuarioAtual.UserId,
                BusinessType = dto.BusinessType,
                BusinessId = (int)dto.BusinessId,
                Rating = (int)Convert.ToDecimal(dto.Rating),
                ReviewContent = dto.ReviewContent,
                ReviewTime = DateTime.Now
            };

            db.Reviews.Add(review);
            await db.SaveChangesAsync();

            // Salva as fotos (se houver)
            if (dto.ImageUrls != null && dto.ImageUrls.Any())
            {
                foreach (var url in dto.ImageUrls)
                    db.ReviewImages.Add(new ReviewImage { ReviewId = review.ReviewId, ImageUrl = url });

                await db.SaveChangesAsync();
            }

            return Ok(new ResponseApi<int>(200, "success", 1)); // Exige retornar 1[cite: 21]
        }

        // 4. EXCLUIR (DELETE system/api/review/{reviewIds})[cite: 21]
        [HttpDelete("{reviewIds}")]
        public async Task<IActionResult> DeleteReviews(
            [FromHeader] string? tokenHeader,
            [FromRoute] string reviewIds
        )
        {
            if (!UsuarioAtual.IsValido(tokenHeader))
                return Ok(new ResponseApi<string>(401, "Token de autorização inválido ou expirado", null));

            var ids = reviewIds.Split(',').Select(long.Parse).ToList();
            var reviews = await db.Reviews
                .Where(r => ids.Contains(r.ReviewId) && r.UserId == UsuarioAtual.UserId)
                .ToListAsync();

            db.Reviews.RemoveRange(reviews);
            int deletados = await db.SaveChangesAsync();

            return Ok(new ResponseApi<int>(200, "success", deletados));
        }
    }
}

// DTOs Enxutos
public class ReviewDTO
{
    public long ReviewId { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string BusinessType { get; set; } = string.Empty;
    public long BusinessId { get; set; }
    public string Rating { get; set; } = string.Empty;
    public string ReviewContent { get; set; } = string.Empty;
    public string ReviewTime { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = new();
    public int LikeCount { get; set; }
}

public class AddReviewDTO
{
    public string BusinessType { get; set; } = string.Empty;
    public long BusinessId { get; set; }
    public string Rating { get; set; } = string.Empty;
    public string ReviewContent { get; set; } = string.Empty;
    public List<string>? ImageUrls { get; set; }
}