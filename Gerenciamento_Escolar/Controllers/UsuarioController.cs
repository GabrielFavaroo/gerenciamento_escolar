using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
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
        return ResponseMapper.createHttpResponse(usuarioUseCases.procurarUm(id),this);

    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult listarUsuarios([FromQuery(Name = "p")][DefaultValue(1)][Range(1,int.MaxValue)]int pagina,
        [FromQuery(Name = "q")][DefaultValue(10)][Range(1,int.MaxValue)]int quantidade)
    {
        return ResponseMapper.createHttpResponse(usuarioUseCases.listar(pagina, quantidade),this);
        
    }
    
    [HttpPost(template:"signin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Authorize(Roles = "Diretor")]
    public IActionResult criarUsuario([FromBody]UsuarioDTO usuarioDto)
    {
        
        return ResponseMapper.createHttpResponse(usuarioUseCases.criar(usuarioDto),this);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Diretor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult deletarUsuario([FromRoute] [Range(1, int.MaxValue)] int id)
    {
        ;
        return ResponseMapper.createHttpResponse(usuarioUseCases.remover(id),this);

    }
    

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Roles = "Diretor")]
    public IActionResult atualizarUsuario([FromRoute][Range(1,int.MaxValue)]int id,[FromBody]UsuarioDTO usuarioDto)
    {
        return ResponseMapper.createHttpResponse(usuarioUseCases.atualizar(id, usuarioDto),this);
    }
    
    
}
