using System.Collections.Generic;
using TurnosDLL.Data.Models;

namespace TuNamespace.Models
{
    public class TurnoRequest
    {
        public TTurno Turno { get; set; }
        public List<TServicio> Servicios { get; set; }
    }
}
