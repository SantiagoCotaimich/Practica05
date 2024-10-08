using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnosDLL.Data.Models;

namespace ServicioDLL.Data.Repositories
{
    public interface ITurnoRepository
    {
        public bool Create(TTurno turno, List<TServicio> servicios);

        public bool Update(int id, TTurno turno, List<TServicio> servicios);

        public bool Delete(int id, string motivoCancelacion);

        TTurno GetById(int? id);

        List<TTurno> GetByTimeCliente(DateTime? fecha, string? cliente);


    }
}
