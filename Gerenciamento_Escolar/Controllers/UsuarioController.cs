using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_Escolar.Controllers;

[Route("usuario")]
[ApiController]
public class UsuarioController(UsuarioUseCases usuarioUseCases) : Controller
{
    
    [HttpGet("{id:int}")]
    public IActionResult encontrarUsuarioPorId([Range(1,int.MaxValue)]int id)
    {
        return Ok(usuarioUseCases.procurarUm(id));

    }

    [HttpGet]
    public IActionResult listarUsuarios([FromQuery(Name = "p")][DefaultValue(1)][Range(1,int.MaxValue)]int pagina,
        [FromQuery(Name = "q")][DefaultValue(10)][Range(1,int.MaxValue)]int quantidade)
    {
        return Ok(usuarioUseCases.listar(pagina, quantidade));
        
    }

    [HttpPost]
    [Authorize(Roles = "diretor")]
    public IActionResult criarUsuario(UsuarioDTO usuarioDto)
    {
        return Created("usuario",usuarioUseCases.criar(usuarioDto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "diretor")]
    public IActionResult deletarUsuario([Range(1,int.MaxValue)]int id)
    {
        usuarioUseCases.remover(id);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "diretor")]
    public IActionResult atualizarUsuario([Range(1,int.MaxValue)]int id,UsuarioDTO usuarioDto)
    {
        return Ok(usuarioUseCases.atualizar(id, usuarioDto));
    }
    
    
}
}