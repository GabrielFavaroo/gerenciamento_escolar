using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_Escolar.Controllers;

[Route("alocacao")]
[ApiController]
public class AlocacaoController(AlocacaoUseCases alocacaoUseCases) : Controller
{
    


    [HttpGet("{id:int}")]
    public IActionResult encontrarAlocacaoPorId([Range(1,int.MaxValue)]int id)
    {

        return Ok(alocacaoUseCases.procurarUm(id));

    }

    [HttpGet]
    public IActionResult listarAlocacoes([FromQuery(Name = "p")][DefaultValue(1)][Range(1,int.MaxValue)]int pagina,
        [FromQuery(Name = "q")][DefaultValue(10)][Range(1,int.MaxValue)]int quantidade)
    {

        return Ok(alocacaoUseCases.listar(pagina, quantidade));
        
    }

    [HttpPost]
    public IActionResult criarAlocacao(AlocacaoDTO alocacaoDto)
    {
        return Created("alocacao",alocacaoUseCases.criar(alocacaoDto));
    }

    [HttpDelete("{id:int}")]
    public IActionResult deletarAlocacao([Range(1,int.MaxValue)]int id)
    {
        alocacaoUseCases.remover(id);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public IActionResult atualizarAlocacao([Range(1,int.MaxValue)]int id,AlocacaoAtualizadaDTO alocacaoDto)
    {
        return Ok(alocacaoUseCases.atualizar(id, alocacaoDto));
    }
    
    
}