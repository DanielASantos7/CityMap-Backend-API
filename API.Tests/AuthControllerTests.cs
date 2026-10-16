using API.Controllers;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Tests
{
    public class AuthControllerTests
    {
        // Fact -> serve para identificar um método que deve ser executado como teste
        [Fact]
        public async Task Login_ComCamposVazios_DeveRetornarBadRequest()
        {

            var options = new DbContextOptionsBuilder<Model1Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new Model1Context(options);

            var controller = new authController(context);

            var dto = new LoginDTO
            {
                Username = "",
                Password = ""
            };

            var resultado = await controller.login(dto);

            var resposta = Assert.IsType<BadRequestObjectResult>(resultado);

            Assert.Equal(400, resposta.StatusCode);

            var corpo = Assert.IsType<ResponseApi<string>>(resposta.Value);

            Assert.Equal(400, corpo.code);
            Assert.Equal("Preencha todos os campos", corpo.msg);
            Assert.Null(corpo.data);
        }

        [Fact]
        public async Task Login_ComCredenciaisInvalidas_DeveRetornarCredenciaisInvalidas()
        {
            var options = new DbContextOptionsBuilder<Model1Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new Model1Context(options);

            var controller = new authController(context);

            var dto = new LoginDTO
            {
                Username = "usuario_inexistente",
                Password = "senha_incorreta"
            };

            var resultado = await controller.login(dto);


            var resposta = Assert.IsType<OkObjectResult>(resultado);

            Assert.Equal(200, resposta.StatusCode);

            var corpo = Assert.IsType<ResponseApi<string>>(resposta.Value);

            Assert.Equal(400, corpo.code);
            Assert.Equal("Credenciais inválidas", corpo.msg);
            Assert.Null(corpo.data);
        }

    }
}
