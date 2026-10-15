using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("/system/api/[controller]")]
    public class authController : ControllerBase
    {
        public readonly Model1Context db;

        public authController(Model1Context context)
        {
            db = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new ResponseApi<string>(400, "Preencha todos os campos", null));

            var usuario = await db.Users.Where(u => u.UserName == dto.Username && u.Password == dto.Password).FirstOrDefaultAsync();

            if (usuario == null)
                return Ok(new ResponseApi<string>(400, "Credenciais inválidas", null));

            UsuarioAtual.UserId = usuario.UserId;
            UsuarioAtual.Token = Guid.NewGuid().ToString("N");
            UsuarioAtual.Expired = DateTime.Now.AddHours(2);


            return Ok(new ResponseApi<object>(200, "success", new { token = UsuarioAtual.Token }));
        }
        [HttpPost("logoff")]
        public async Task<IActionResult> logoff()
        {
            UsuarioAtual.UserId = 0;
            UsuarioAtual.Token = null;
            UsuarioAtual.Expired = DateTime.MinValue;


            return Ok(new ResponseApi<string>(200, "success", null));
        }
    }
}


public class LoginDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
}