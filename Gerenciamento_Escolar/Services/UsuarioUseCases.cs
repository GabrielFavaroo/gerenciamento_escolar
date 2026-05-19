using System.Text;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Infrastructure;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Services;

public class UsuarioUseCases(Context context, TokenService tokenService)
{
    

    public Usuario criar(UsuarioDTO usuarioDto)
    {
        if (!Enum.TryParse<CargosDeUsuario>(usuarioDto.tipoUsuario, true, out CargosDeUsuario resultado))
        {
            throw new Exception("Cargo de usuario invalido");
        } 
        
        //if (nameof(resultado).Equals(nameof(CargosDeUsuario.Diretor)) )
        //{
        //    throw new Exception(message: "Não é permitido criar um usuario com este cargo");}

        if (context.Usuarios.Any(u => u.nome == usuarioDto.nome))
        {
            throw new Exception($"O nome {usuarioDto.nome} já está em uso");
        };
        var key = Configuration.encodingKey;
        

        var senhaEncoded = tokenService.hasherDeSenha(usuarioDto.senha, Encoding.ASCII.GetBytes(key));
        
        var usuario = new Usuario(usuarioDto.nome,
            usuarioDto.email,
            senhaEncoded, resultado.ToString());
        context.Usuarios.Add(usuario);
        context.SaveChanges();
        
        return usuario;
    }
    public Usuario procurarUm(int id)
    {
        var usuario = context.Usuarios.Find(id);
        if (usuario == null)
        {
            throw new Exception("Usuario não encontrado");}
        return usuario;
    }
    public ListaDeUsuarioDTO listar(int pagina, int quantidade)
    {
        var usuarios = context.Usuarios.Skip((pagina - 1) * quantidade).Take(quantidade)
            .Select(u => new RespostaUsuarioDTO(u.id, u.nome, u.email, u.tipoUsuario)).ToList();
        return new ListaDeUsuarioDTO(pagina,quantidade,usuarios);
    }
    public void remover(int id)
    {
        
        context.Usuarios.Where(u => u.id == id).ExecuteDelete();
    }
    
    public Usuario atualizar(int id, UsuarioDTO usuarioAtualizadoDto)
    {   
        
        var key = Configuration.encodingKey;
        

        var senhaEncoded = tokenService.hasherDeSenha(usuarioAtualizadoDto.senha, Encoding.ASCII.GetBytes(key));

        var usuarioAtualizado = new Usuario(id,usuarioAtualizadoDto.nome, usuarioAtualizadoDto.email, senhaEncoded, usuarioAtualizadoDto.tipoUsuario);
        context.Usuarios.Update(usuarioAtualizado);
        context.SaveChanges();
        return usuarioAtualizado;
    }


}