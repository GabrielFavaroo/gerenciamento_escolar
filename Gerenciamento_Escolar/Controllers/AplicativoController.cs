using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_Escolar.Controllers;

[ApiController]
[Route("aplicativo")]

public class AplicativoController(AplicativoUseCases aplicativoUseCases) : ControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult encontrarAplicativoPorId([FromRoute][Range(1,int.MaxValue)]int id)
    {
        return Ok(aplicativoUseCases.procurarUm(id));

    }

    [HttpGet]
    [Authorize(Roles = "Tecnico")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult listarAplicativos([FromQuery(Name = "p")][DefaultValue(1)][Range(1,int.MaxValue)]int pagina,
        [FromQuery(Name = "q")][DefaultValue(10)][Range(1,int.MaxValue)]int quantidade)
    {
        return Ok(aplicativoUseCases.listar(pagina, quantidade));
        
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Authorize(Roles = "Tecnico")]
    public IActionResult criarAplicativo([FromBody]AplicativoDTO aplicativoDto)
    {
        return Created("aplicativo",aplicativoUseCases.criar(aplicativoDto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Tecnico")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult deletarAplicativo([FromRoute][Range(1,int.MaxValue)]int id)
    {
        aplicativoUseCases.remover(id);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Roles = "Tecnico")]
    public IActionResult atualizarAplicativo([FromRoute][Range(1,int.MaxValue)]int id,[FromBody]AplicativoDTO aplicativoDto)
    {
        return Ok(aplicativoUseCases.atualizar(id, aplicativoDto));
    }
    
    
}