using SpaceConnect.ApiService.Models;

namespace SpaceConnect.ApiService.Data.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> BuscarPorEmailAsync(string email);
    Task<Usuario?> BuscarPorIdAsync(int id);
    Task<Usuario> CriarAsync(Usuario usuario);
    Task<bool> EmailExisteAsync(string email);
}
