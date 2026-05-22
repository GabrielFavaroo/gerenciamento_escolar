using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_Escolar.Controllers;

[ApiController]
[Route("alocacao")]

public class AlocacaoController(AlocacaoUseCases alocacaoUseCases) : ControllerBase
{
    


    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult encontrarAlocacaoPorId([FromRoute][Range(1,int.MaxValue)]int id)
    {

        return ResponseMapper.createHttpResponse(alocacaoUseCases.ProcurarUm(id),this);

    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult listarAlocacoes([FromQuery(Name = "p")][DefaultValue(1)][Range(1,int.MaxValue)]int pagina,
        [FromQuery(Name = "q")][DefaultValue(10)][Range(1,int.MaxValue)]int quantidade)
    {

        return ResponseMapper.createHttpResponse(alocacaoUseCases.Listar(pagina, quantidade),this);
        
    }

    [HttpPost]
    [Authorize(Roles = "Coordenador")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult criarAlocacao([FromBody]AlocacaoDTO alocacaoDto)
    {
        return ResponseMapper.createHttpResponse(alocacaoUseCases.Criar(alocacaoDto),this);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Diretor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult deletarAlocacao([FromRoute][Range(1,int.MaxValue)]int id)
    {
        
        return ResponseMapper.createHttpResponse(alocacaoUseCases.remover(id),this);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Diretor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult atualizarAlocacao([FromRoute][Range(1,int.MaxValue)]int id,[FromBody]AlocacaoAtualizadaDTO alocacaoDto)
    {
        return ResponseMapper.createHttpResponse(alocacaoUseCases.Atualizar(id, alocacaoDto),this);
    }
    
    
}