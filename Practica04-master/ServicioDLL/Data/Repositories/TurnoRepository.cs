using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnosDLL.Data.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServicioDLL.Data.Repositories
{
    public class TurnoRepository : ITurnoRepository
    {


        private TurnosDBContext _context;

        public TurnoRepository(TurnosDBContext context)
        {
            _context = context;
        }
        public bool Create(TTurno turno, List<TServicio> servicios)
        {
            var fechaActual = DateTime.Now.Date;
            DateTime fechaTurno;
            if (turno.Fecha == null ||
                !DateTime.TryParse(turno.Fecha, out fechaTurno) ||
                fechaTurno < fechaActual.AddDays(1) ||
                fechaTurno > fechaActual.AddDays(45))
            {
                return false;
            }

            if (servicios == null || servicios.Count == 0)
            {
                return false;
            }

            var serviciosUnicos = servicios.GroupBy(s => s.Nombre).All(g => g.Count() == 1);
            if (!serviciosUnicos)
            {
                return false;
            }

            var existeTurno = _context.TTurnos.Any(t =>
                t.Fecha == turno.Fecha &&
                t.Hora == turno.Hora);
            if (existeTurno)
            {
                return false;
            }

            _context.TTurnos.Add(turno);
            _context.SaveChanges(); 

            foreach (var servicio in servicios)
            {
                var detalleTurno = new TDetallesTurno
                {
                    IdTurno = turno.Id, 
                    IdServicio = servicio.Id,
                };

                _context.TDetallesTurnos.Add(detalleTurno);
            }

            _context.SaveChanges();

            return true;
        }



        public bool Delete(int id, string motivoCancelacion)
        {
            var turno = GetById(id);

            if (turno == null)
            {
                return false;
            }


            turno.FechaCancelacion = DateTime.Now;
            turno.MotivoCancelacion = motivoCancelacion;

            _context.TTurnos.Update(turno);
            _context.SaveChanges();

            return true;
        }


        public TTurno GetById(int? id)
        {
            return _context.TTurnos.Find(id);
        }

        public List<TTurno> GetByTimeCliente(DateTime? fecha, string? cliente)
        {
            var query = _context.TTurnos.AsQueryable();

            if (!string.IsNullOrEmpty(cliente))
            {
                query = query.Where(t => t.Cliente.Contains(cliente));
            }

            if (fecha.HasValue)
            {
                query = query.Where(t => t.Fecha == fecha.Value.ToString("yyyy-MM-dd"));
            }

            return query.ToList();
        }


        public bool Update(int id, TTurno turno, List<TServicio> servicios)
        {
            var turnoExistente = _context.TTurnos.Find(id);
            if (turnoExistente == null)
            {
                return false; 
            }

            var fechaActual = DateTime.Now.Date;


            DateTime fechaTurno;
            if (turno.Fecha == null ||
                !DateTime.TryParse(turno.Fecha, out fechaTurno) ||
                fechaTurno < fechaActual.AddDays(1) ||
                fechaTurno > fechaActual.AddDays(45))
            {
                return false;
            }


            if (servicios == null || servicios.Count == 0)
            {
                return false;
            }


            var serviciosUnicos = servicios.GroupBy(s => s.Nombre).All(g => g.Count() == 1);
            if (!serviciosUnicos)
            {
                return false;
            }


            var existeTurno = _context.TTurnos.Any(t =>
                t.Fecha == turno.Fecha &&
                t.Hora == turno.Hora &&
                t.Id != id);
            if (existeTurno)
            {
                return false;
            }

            turnoExistente.Fecha = turno.Fecha;
            turnoExistente.Hora = turno.Hora;
            turnoExistente.Cliente = turno.Cliente;

            _context.TTurnos.Update(turnoExistente);
            _context.SaveChanges();

            return true;
        }

    }
}
