using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;

namespace Gerenciamento_Escolar.Services;

public class UsuarioUseCases(Context context)
{
    

    public Usuario criar(UsuarioDTO usuarioDto)
    {   
        var usuario = new Usuario(usuarioDto.nome,
            usuarioDto.versao,
            usuarioDto.descricao);
        context.Usuarios.Add(usuario);
        usuario = context.Usuarios.Find(usuario);
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
    public ListaDeUsuariosDTO listar(int pagina, int quantidade)
    {
        var usuarios = context.Usuarios.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();
        return new ListaDeUsuariosDTO(pagina,quantidade,usuarios);
    }
    public void remover(int id)
    {
        
        context.Usuarios.Remove(this.procurarUm(id));
    }
    
    public Usuario atualizar(int id, UsuarioDTO usuarioAtualizadoDto)
    {
        var usuarioAtualizado = new Usuario(id, usuarioAtualizadoDto.nome, usuarioAtualizadoDto.versao,
            usuarioAtualizadoDto.descricao);
        context.Usuarios.Update(usuarioAtualizado);
        var usuario = context.Usuarios.Find(id);
        return usuario;
    }


}