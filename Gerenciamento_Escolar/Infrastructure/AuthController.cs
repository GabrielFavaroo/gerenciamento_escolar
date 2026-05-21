using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_Escolar.Infrastructure;

[ApiController]
[Route("auth")]
public class AuthController(TokenService tokenService) : ControllerBase
{
    
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    
    public IActionResult login([FromBody]CredenciaisDeUsuarioDTO usuarioDto)
    {

        return ResponseMapper.createHttpResponse(tokenService.createToken(new Usuario(usuarioDto.nome, usuarioDto.senha)),this);
    }

    
    
}