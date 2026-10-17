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


        [Fact]
        public async Task Login_ComCredenciaisValidas_DeveRetornarToken() 
        {
            var options = new DbContextOptionsBuilder<Model1Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new Model1Context(options);
            var controller = new authController(context);

            context.Users.Add(new User { UserName = "usuario_valido", Password = "senha_valida", Email = "email_valido@email.com" });

            await context.SaveChangesAsync();

            var dto = new LoginDTO
            {
                Username = "usuario_valido",
                Password = "senha_valida"
            };

            var resultado = await controller.login(dto);

            var resposta = Assert.IsType<OkObjectResult>(resultado);

            Assert.Equal(200, resposta.StatusCode);

            var corpo = Assert.IsType<ResponseApi<object>>(resposta.Value);

            Assert.Equal(200, corpo.code);
            Assert.Equal("success", corpo.msg);
            Assert.NotNull(corpo.data);

            var dados = corpo.data;

            var propriedadeToken = dados.GetType().GetProperty("token");
            Assert.NotNull(propriedadeToken);

            var token = propriedadeToken.GetValue(dados) as string;
            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public async Task Logoff_DeveLimparSessaoERetornarSucesso()
        {
            var options = new DbContextOptionsBuilder<Model1Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new Model1Context(options);

            UsuarioAtual.UserId = 123;
            UsuarioAtual.Token = "tokens_teste_123";
            UsuarioAtual.Expired = DateTime.Now.AddHours(2);

            var controller = new authController(context);

            var resultado = await controller.logoff();

            var resposta = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, resposta.StatusCode);

            var corpo = Assert.IsType<ResponseApi<string>>(resposta.Value);

            Assert.Equal(200, corpo.code);
            Assert.Equal("success", corpo.msg);
            Assert.Null(corpo.data);

            Assert.Equal(0, UsuarioAtual.UserId);
            Assert.Null(UsuarioAtual.Token);
            Assert.Equal(DateTime.MinValue, UsuarioAtual.Expired);
        }
    }
}
