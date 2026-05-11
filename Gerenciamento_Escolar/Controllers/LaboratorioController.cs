using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_Escolar.Controllers;

[Route("laboratorio")]
[ApiController]
public class LaboratorioController(LaboratorioUseCases laboratorioUseCases) : Controller
{
    
    [HttpGet("{id:int}")]
    public IActionResult encontrarLaboratorioPorId([Range(1,int.MaxValue)]int id)
    {
        return Ok(laboratorioUseCases.procurarUm(id));

    }

    [HttpGet]
    public IActionResult listarLaboratorios([FromQuery(Name = "p")][DefaultValue(1)][Range(1,int.MaxValue)]int pagina,
        [FromQuery(Name = "q")][DefaultValue(10)][Range(1,int.MaxValue)]int quantidade)
    {
        return Ok(laboratorioUseCases.listar(pagina, quantidade));
        
    }

    [HttpPost]
    [Authorize(Roles = "diretor")]
    public IActionResult criarLaboratorio(LaboratorioDTO laboratorioDto)
    {
        return Created("laboratorio", laboratorioUseCases.criar(laboratorioDto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "diretor")]
    public IActionResult deletarLaboratorio([Range(1,int.MaxValue)]int id)
    {
        laboratorioUseCases.remover(id);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "tecnico,diretor")]
    public IActionResult atualizarLaboratorio([Range(1,int.MaxValue)]int id,LaboratorioDTO laboratorioDto)
    {
        return Ok(laboratorioUseCases.atualizar(id, laboratorioDto));
    }
    
    
}