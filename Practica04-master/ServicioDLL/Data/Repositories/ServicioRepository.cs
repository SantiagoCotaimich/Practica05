using TurnosDLL.Data.Models;

namespace ServicioDLL.Data.Repositories
{
    public class ServicioRepository : IServicioRepository
    {

        private TurnosDBContext _context;

        public ServicioRepository(TurnosDBContext context)
        {
            _context = context;
        }



        public TServicio? GetById(int id)
        {
            return _context.TServicios.Find(id);
        }

        public List<TServicio> GetByNameCost(string? nombre = null, int? costo = null)
        {
            var query = _context.TServicios.AsQueryable();


            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(s => s.Nombre.Contains(nombre));
            }


            if (costo.HasValue)
            {
                query = query.Where(s => s.Costo == costo.Value);
            }


            return query.ToList();
        }
    }
}
