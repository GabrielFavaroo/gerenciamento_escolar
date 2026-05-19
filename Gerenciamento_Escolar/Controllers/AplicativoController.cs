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
    public IActionResult encontrarAplicativoPorId([Range(1,int.MaxValue)]int id)
    {
        return Ok(aplicativoUseCases.procurarUm(id));

    }

    [HttpGet]
    [Authorize(Roles = "Tecnico")]
    public IActionResult listarAplicativos([FromQuery(Name = "p")][DefaultValue(1)][Range(1,int.MaxValue)]int pagina,
        [FromQuery(Name = "q")][DefaultValue(10)][Range(1,int.MaxValue)]int quantidade)
    {
        return Ok(aplicativoUseCases.listar(pagina, quantidade));
        
    }

    [HttpPost]
    [Authorize(Roles = "Tecnico")]
    public IActionResult criarAplicativo(AplicativoDTO aplicativoDto)
    {
        return Created("aplicativo",aplicativoUseCases.criar(aplicativoDto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Tecnico")]
    public IActionResult deletarAplicativo([Range(1,int.MaxValue)]int id)
    {
        aplicativoUseCases.remover(id);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Tecnico")]
    public IActionResult atualizarAplicativo([Range(1,int.MaxValue)]int id,AplicativoDTO aplicativoDto)
    {
        return Ok(aplicativoUseCases.atualizar(id, aplicativoDto));
    }
    
    
}