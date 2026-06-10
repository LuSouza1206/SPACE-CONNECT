using Microsoft.EntityFrameworkCore;
using SpaceConnect.ApiService.Models;

namespace SpaceConnect.ApiService.Data;

public static class DemoUserSeed
{
    public static async Task ApplyAsync(SpaceConnectContext db)
    {
        if (await db.Usuarios.AnyAsync())
            return;

        db.Usuarios.AddRange(
            new Usuario
            {
                Nome = "Administrador Demo",
                Email = "admin@spaceconnect.fiap",
                SenhaHash = "$2a$11$MWL9vTeCw1Wz2q6c2caAAe5c6lbYSXp00e6sF0qWPvK8McDA3JgHO",
                Perfil = "Administrador",
                Ativo = true
            },
            new Usuario
            {
                Nome = "Pesquisador Demo",
                Email = "pesquisador@spaceconnect.fiap",
                SenhaHash = "$2a$11$e50MSvd7PxsOKslifK5nLuvMEqD6xqfR8PshM94qlPoAp5naeBLj2",
                Perfil = "Pesquisador",
                Ativo = true
            });

        await db.SaveChangesAsync();
    }
}
