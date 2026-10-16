using API.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class uploadController : ControllerBase
    {

        [HttpPost("imagens")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> files)
        {

            if (files == null)
                return Ok(new ResponseApi<string>(400, "Nenhum arquivo enviado", null));

            string pastaData = DateTime.Now.ToString("yyyyMMdd");
            string caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", pastaData);

            Directory.CreateDirectory(caminhoPasta);

            var detalhes = new List<object>();

            foreach (var arquivo in files)
            {
                string nomeUnico = $"{DateTime.Now.Ticks}_{arquivo.FileName}";
                string caminhoCompleto = Path.Combine(caminhoPasta, nomeUnico);

                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }

                string caminhoRelativo = $"/upload/{pastaData}/{nomeUnico}";
                string urlCompleta = $"{Request.Scheme}://{Request.Host}{caminhoRelativo}";

                detalhes.Add(new
                {
                    fileName = caminhoRelativo,
                    url = urlCompleta,
                    size = arquivo.Length
                });
            }

            var payload = new
            {
                success = detalhes.Count,
                fail = 0,
                total = files.Count,
                details = detalhes
            };

            return Ok(new ResponseApi<object>(200, "succes", payload));
        }

    }

}
