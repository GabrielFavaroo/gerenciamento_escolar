using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_Escolar.Controllers;

[ApiController]
[Route("disciplina")]

public class DisciplinaController(DisciplinaUseCases disciplinaUseCases) : ControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult encontrarDisciplinaPorId([FromRoute][Range(1,int.MaxValue)]int id)
    {
        return Ok(disciplinaUseCases.procurarUm(id));

    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult listarDisciplinas([FromQuery(Name = "p")][DefaultValue(1)][Range(1,int.MaxValue)]int pagina,
        [FromQuery(Name = "q")][DefaultValue(10)][Range(1,int.MaxValue)]int quantidade)
    {
        return Ok(disciplinaUseCases.listar(pagina, quantidade));
        
    }

    [HttpPost]
    [Authorize(Roles = "Coordenador")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult criarDisciplina([FromBody]DisciplinaDTO disciplinaDto)
    {
        return Created("disciplina",disciplinaUseCases.criar(disciplinaDto));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Coordenador")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult deletarDisciplina([FromRoute][Range(1,int.MaxValue)]int id)
    {
        disciplinaUseCases.remover(id);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Coordenador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult atualizarDisciplina([FromRoute][Range(1,int.MaxValue)]int id,[FromBody]DisciplinaDTO disciplinaDto)
    {
        return Ok(disciplinaUseCases.atualizar(id, disciplinaDto));
    }
    
    
}