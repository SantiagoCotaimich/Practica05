using TurnosDLL.Data.Models;

namespace ServicioDLL.Data.Repositories
{
    public interface IServicioRepository
    {
        TServicio? GetById(int id);
        List<TServicio> GetByNameCost(string? nombre, int? costo);
    }
}
