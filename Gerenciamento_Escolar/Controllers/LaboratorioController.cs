using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_Escolar.Controllers;

[ApiController]
[Route("laboratorio")]

public class LaboratorioController(LaboratorioUseCases laboratorioUseCases) : ControllerBase
{
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult encontrarLaboratorioPorId([FromRoute][Range(1,int.MaxValue)]int id)
    {
        return ResponseMapper.createHttpResponse(laboratorioUseCases.procurarUm(id),this);

    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult listarLaboratorios([FromQuery(Name = "p")][DefaultValue(1)][Range(1,int.MaxValue)]int pagina,
        [FromQuery(Name = "q")][DefaultValue(10)][Range(1,int.MaxValue)]int quantidade)
    {
        return ResponseMapper.createHttpResponse(laboratorioUseCases.listar(pagina, quantidade),this);
        
    }

    [HttpPost]
    [Authorize(Roles = "Diretor")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult criarLaboratorio([FromBody]LaboratorioDTO laboratorioDto)
    {
        return ResponseMapper.createHttpResponse(laboratorioUseCases.criar(laboratorioDto),this);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Diretor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult deletarLaboratorio([FromRoute][Range(1,int.MaxValue)]int id)
    {
        return ResponseMapper.createHttpResponse(laboratorioUseCases.remover(id),this);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Tecnico,Diretor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult atualizarLaboratorio([FromRoute][Range(1,int.MaxValue)]int id,[FromBody]LaboratorioDTO laboratorioDto)
    {
        return ResponseMapper.createHttpResponse(laboratorioUseCases.atualizar(id, laboratorioDto),this);
    }

    [HttpPost("vincular_apps")]
    [Authorize(Roles = "Tecnico")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult vincularApps([FromBody] VincularAppsNoLaboratorioDTO laboratorioDto)
    {
        return ResponseMapper.createHttpResponse(laboratorioUseCases.vincular(laboratorioDto), this);
    }
    
    
}