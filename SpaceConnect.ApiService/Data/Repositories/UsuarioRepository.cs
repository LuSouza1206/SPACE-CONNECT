using Microsoft.EntityFrameworkCore;
using SpaceConnect.ApiService.Data.Interfaces;
using SpaceConnect.ApiService.Models;

namespace SpaceConnect.ApiService.Data.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly SpaceConnectContext _context;

    public UsuarioRepository(SpaceConnectContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> BuscarPorEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email.ToLower() && u.Ativo);
    }

    public async Task<Usuario?> BuscarPorIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<Usuario> CriarAsync(Usuario usuario)
    {
        usuario.Email = usuario.Email.ToLower();
        usuario.CriadoEm = DateTime.UtcNow;
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<bool> EmailExisteAsync(string email)
    {
        return await _context.Usuarios.AnyAsync(u => u.Email == email.ToLower());
    }
}
