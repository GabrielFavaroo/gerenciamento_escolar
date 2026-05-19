using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_Escolar.Infrastructure;

[ApiController]
[Route("auth")]
public class AuthController(TokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult login(CredenciaisDeUsuarioDTO usuarioDto)
    {

        return Ok(tokenService.receberToken(new Usuario(usuarioDto.nome, usuarioDto.senha)));
    }

    
    
}