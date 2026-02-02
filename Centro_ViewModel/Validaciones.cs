using Centro_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Centro_ViewModel
{
    public class Validaciones
    {
        //VAlidaciones para socios
        public static bool NombreValido(string nombre, out string error)
        {
            error = "";

            if (string.IsNullOrWhiteSpace(nombre))
            {
                error = "El nombre no puede estar vacio";
                return false;
            }

            if (nombre.Trim().Length < 2)
            {
                error = "El nombre es demasiado corto";
                return false;
            }

            return true;
        }

        public static bool EmailValido(string email, out string error)
        {
            error = "";

            if (string.IsNullOrWhiteSpace(email))
            {
                error = "El Email no puede estar vacio";
                return false;
            }

            email = email.Trim();

            // valida formato basico sin volverte loco
            // evita cosas tipo "a@b" sin dominio y "aa@@bb"
            var patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, patron))
            {
                error = "El Email no tiene un formato correcto";
                return false;
            }

            return true;
        }

        public static bool SocioFormularioValido(string nombre, string email, out string error)
        {
            error = "";

            if (!NombreValido(nombre, out error)) return false;
            if (!EmailValido(email, out error)) return false;

            return true;
        }

        public static string LimpiarTexto(string texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? "" : texto.Trim();
        }

        //Validaciones para reservas 
        public static bool ReservaFormularioValido(
            int socioId,
            int actividadId,
            DateTime fecha,
            List<Actividades> listaActividades,
            List<Reservas> listaReservas,
            out string error,
            int? reservaIdExcluida = null
        )
        {
            error = "";

            if (socioId == -1)
            {
                error = "Elige un socio";
                return false;
            }

            if (actividadId == -1)
            {
                error = "Elige una actividad";
                return false;
            }

            var actividad = listaActividades?.FirstOrDefault(a => a.Id == actividadId);
            if (actividad == null)
            {
                error = "Elige una actividad";
                return false;
            }

            if (fecha.Date < DateTime.Today)
            {
                error = "La fecha no puede ser anterior a hoy.";
                return false;
            }

            // contar reservas de esa actividad, excluyendo la reserva actual si estamos modificando
            int cantidadReservas = 0;
            if (listaReservas != null)
            {
                foreach (var r in listaReservas)
                {
                    if (r.ActividadId == actividadId)
                    {
                        if (reservaIdExcluida.HasValue && r.Id == reservaIdExcluida.Value)
                            continue;

                        cantidadReservas++;
                    }
                }
            }

            if (cantidadReservas >= actividad.AforoMaximo)
            {
                error = "El aforo maximo ya se ha llenado";
                return false;
            }

            return true;
        }

        //Validaciones para las actividades 
        public static bool ActividadFormularioValido(string nombre, string aforoMaxTexto, out string error, out int aforo)
        {
            error = "";
            aforo = 0;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                error = "El nombre no puede estar vacio";
                return false;
            }

            if (!int.TryParse(aforoMaxTexto, out aforo) || aforo <= 0)
            {
                error = "El aforo tiene que ser un numero positivo";
                return false;
            }

            return true;
        }

    }
}
