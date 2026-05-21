using System.Text;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Infrastructure;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Services;

public class UsuarioUseCases(Context context,HashServices hashServices)
{
    

    public Result<RespostaUsuarioDTO> criar(UsuarioDTO usuarioDto)
    {
        if (!Enum.TryParse<CargosDeUsuario>(usuarioDto.tipoUsuario, true, out CargosDeUsuario resultado))
        {
            return Result<RespostaUsuarioDTO>.Failure("Cargo de usuario invalido",403);
        } 
        
        if (resultado.ToString().Equals(nameof(CargosDeUsuario.Diretor)) )
        {
            return Result<RespostaUsuarioDTO>.Failure("Não é permitido Criar um usuario com este cargo",403);}

        if (context.Usuarios.Any(u => u.nome == usuarioDto.nome))
        {
            return Result<RespostaUsuarioDTO>.Failure($"O nome {usuarioDto.nome} já está em uso",409);
        };
        
        var senhaEncoded = hashServices.toHashPassword(usuarioDto.senha);
        
        var usuario = new Usuario(usuarioDto.nome,
            usuarioDto.email,
            senhaEncoded, resultado.ToString());
        context.Usuarios.Add(usuario);
        context.SaveChanges();

        var returnUser = new RespostaUsuarioDTO(usuario.id, usuario.nome, usuario.email, usuario.tipoUsuario);
        
        return Result<RespostaUsuarioDTO>.Success(returnUser,201);
    }
    public Result<RespostaUsuarioDTO> procurarUm(int id)
    {
        var usuario = context.Usuarios.Find(id);
        if (usuario == null)
        {
            return Result<RespostaUsuarioDTO>.Failure("Usuario não encontrado",404);
        }

        var returnUser = new RespostaUsuarioDTO(usuario.id, usuario.nome, usuario.email, usuario.tipoUsuario);
            
        return Result<RespostaUsuarioDTO>.Success(returnUser,200);
    }
    public Result<ListaDeUsuarioDTO>listar(int pagina, int quantidade)
    {
        var usuarios = context.Usuarios.Skip((pagina - 1) * quantidade).Take(quantidade)
            .Select(u => new RespostaUsuarioDTO(u.id, u.nome, u.email, u.tipoUsuario)).ToList();
        return Result<ListaDeUsuarioDTO>.Success(new ListaDeUsuarioDTO(pagina, quantidade, usuarios),200);
    }
    
    public Result<Usuario> remover(int id)
    {
        
        context.Usuarios.Where(u => u.id == id).ExecuteDelete();
        return Result<Usuario>.NoContent(204);
    }
    
    public Result<RespostaUsuarioDTO> atualizar(int id, UsuarioDTO usuarioAtualizadoDto)
    {   
        
        var key = Configuration.encodingKey;


        var senhaHashed = hashServices.toHashPassword(usuarioAtualizadoDto.senha);

        var usuarioAtualizado = new Usuario(id,usuarioAtualizadoDto.nome, usuarioAtualizadoDto.email, senhaHashed, usuarioAtualizadoDto.tipoUsuario);
        context.Usuarios.Update(usuarioAtualizado);
        context.SaveChanges();
        
        var returnUser  = new RespostaUsuarioDTO(usuarioAtualizado.id, usuarioAtualizado.nome, usuarioAtualizado.email, usuarioAtualizado.tipoUsuario);
        return Result<RespostaUsuarioDTO>.Success(returnUser,200);
    }


}