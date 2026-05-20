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
    public IActionResult encontrarAlocacaoPorId([Range(1,int.MaxValue)]int id)
    {

        return ResponseMapper.createHttpResponse(alocacaoUseCases.ProcurarUm(id),this);

    }

    [HttpGet]
    public IActionResult listarAlocacoes([FromQuery(Name = "p")][DefaultValue(1)][Range(1,int.MaxValue)]int pagina,
        [FromQuery(Name = "q")][DefaultValue(10)][Range(1,int.MaxValue)]int quantidade)
    {

        return ResponseMapper.createHttpResponse(alocacaoUseCases.Listar(pagina, quantidade),this);
        
    }

    [HttpPost]
    [Authorize(Roles = "coordenador")]
    public IActionResult criarAlocacao(AlocacaoDTO alocacaoDto)
    {
        return ResponseMapper.createHttpResponse(alocacaoUseCases.Criar(alocacaoDto),this);
    }

    [HttpDelete("{id:int}")]
    public IActionResult deletarAlocacao([Range(1,int.MaxValue)]int id)
    {
        
        return ResponseMapper.createHttpResponse(alocacaoUseCases.remover(id),this);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "diretor")]
    public IActionResult atualizarAlocacao([Range(1,int.MaxValue)]int id,AlocacaoAtualizadaDTO alocacaoDto)
    {
        return ResponseMapper.createHttpResponse(alocacaoUseCases.Atualizar(id, alocacaoDto),this);
    }
    
    
}