using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_Escolar.Controllers;

[ApiController]
[Route("user")]

public class UsuarioController(UsuarioUseCases usuarioUseCases) : ControllerBase
{
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult encontrarUsuarioPorId([FromRoute][Range(1,int.MaxValue)]int id)
    {
        return Ok(usuarioUseCases.procurarUm(id));

    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult listarUsuarios([FromQuery(Name = "p")][DefaultValue(1)][Range(1,int.MaxValue)]int pagina,
        [FromQuery(Name = "q")][DefaultValue(10)][Range(1,int.MaxValue)]int quantidade)
    {
        return Ok(usuarioUseCases.listar(pagina, quantidade));
        
    }
    
    [HttpPost(template:"signin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    
    //[Authorize(Roles = "Diretor")]
    [AllowAnonymous]
    public IActionResult criarUsuario([FromBody]UsuarioDTO usuarioDto)
    {
        return Created("usuario",usuarioUseCases.criar(usuarioDto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Diretor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult deletarUsuario([FromRoute][Range(1,int.MaxValue)]int id)
    {
        usuarioUseCases.remover(id);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Roles = "Diretor")]
    public IActionResult atualizarUsuario([FromRoute][Range(1,int.MaxValue)]int id,[FromBody]UsuarioDTO usuarioDto)
    {
        return Ok(usuarioUseCases.atualizar(id, usuarioDto));
    }
    
    
}
