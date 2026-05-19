using System.Text;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Infrastructure;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Services;

public class UsuarioUseCases(Context context,HashServices hashServices)
{
    

    public Result<Usuario> criar(UsuarioDTO usuarioDto)
    {
        if (!Enum.TryParse<CargosDeUsuario>(usuarioDto.tipoUsuario, true, out CargosDeUsuario resultado))
        {
            return Result<Usuario>.Failure("Cargo de usuario invalido");
        } 
        
        if (resultado.ToString().Equals(nameof(CargosDeUsuario.Diretor)) )
        {
            return Result<Usuario>.Failure("Não é permitido criar um usuario com este cargo");}

        if (context.Usuarios.Any(u => u.nome == usuarioDto.nome))
        {
            return Result<Usuario>.Failure($"O nome {usuarioDto.nome} já está em uso");
        };
        
        var senhaEncoded = hashServices.toHashPassword(usuarioDto.senha);
        
        var usuario = new Usuario(usuarioDto.nome,
            usuarioDto.email,
            senhaEncoded, resultado.ToString());
        context.Usuarios.Add(usuario);
        context.SaveChanges();
        
        return Result<Usuario>.Success(usuario);
    }
    public Result<Usuario> procurarUm(int id)
    {
        var usuario = context.Usuarios.Find(id);
        if (usuario == null)
        {
            return Result<Usuario>.Failure("Usuario não encontrado");
        }
        return Result<Usuario>.Success(usuario);
    }
    public Result<ListaDeUsuarioDTO>listar(int pagina, int quantidade)
    {
        var usuarios = context.Usuarios.Skip((pagina - 1) * quantidade).Take(quantidade)
            .Select(u => new RespostaUsuarioDTO(u.id, u.nome, u.email, u.tipoUsuario)).ToList();
        return Result<ListaDeUsuarioDTO>.Success(new ListaDeUsuarioDTO(pagina, quantidade, usuarios));
    }
    
    public void remover(int id)
    {
        
        context.Usuarios.Where(u => u.id == id).ExecuteDelete();
    }
    
    public Result<Usuario> atualizar(int id, UsuarioDTO usuarioAtualizadoDto)
    {   
        
        var key = Configuration.encodingKey;


        var senhaHashed = hashServices.toHashPassword(usuarioAtualizadoDto.senha);

        var usuarioAtualizado = new Usuario(id,usuarioAtualizadoDto.nome, usuarioAtualizadoDto.email, senhaHashed, usuarioAtualizadoDto.tipoUsuario);
        context.Usuarios.Update(usuarioAtualizado);
        context.SaveChanges();
        return Result<Usuario>.Success(usuarioAtualizado);
    }


}