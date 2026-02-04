using Centro_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Centro_ViewModel
{
    /// <summary>
    /// Clase con metodos de validacion usados por los viewmodels.
    /// Contiene validaciones de socios, reservas y actividades.
    /// </summary>
    public class Validaciones
    {
        //VAlidaciones para socios
        /// <summary>
        /// Valida el nombre de un socio.
        /// </summary>
        /// <param name="nombre">Nombre a validar.</param>
        /// <param name="error">Mensaje de error devuelto si no es valido.</param>
        /// <returns>True si el nombre es valido, false en caso contrario.</returns>
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

        /// <summary>
        /// Valida el formato basico de un email.
        /// </summary>
        /// <param name="email">Email a validar.</param>
        /// <param name="error">Mensaje de error devuelto si no es valido.</param>
        /// <returns>True si el email es valido, false en caso contrario.</returns>
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

        /// <summary>
        /// Valida el formulario de socio combinando validaciones de nombre y email.
        /// </summary>
        /// <param name="nombre">Nombre a validar.</param>
        /// <param name="email">Email a validar.</param>
        /// <param name="error">Mensaje de error devuelto si no es valido.</param>
        /// <returns>True si el formulario es valido, false en caso contrario.</returns>
        public static bool SocioFormularioValido(string nombre, string email, out string error)
        {
            error = "";

            if (!NombreValido(nombre, out error)) return false;
            if (!EmailValido(email, out error)) return false;

            return true;
        }

        /// <summary>
        /// Limpia un texto eliminando espacios alrededor o devolviendo cadena vacia si es nulo o whitespace.
        /// </summary>
        /// <param name="texto">Texto a limpiar.</param>
        /// <returns>Texto limpio o cadena vacia.</returns>
        public static string LimpiarTexto(string texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? "" : texto.Trim();
        }

        //Validaciones para reservas 
        /// <summary>
        /// Valida los datos para crear o modificar una reserva.
        /// </summary>
        /// <param name="socioId">Id del socio seleccionado.</param>
        /// <param name="actividadId">Id de la actividad seleccionada.</param>
        /// <param name="fecha">Fecha de la reserva.</param>
        /// <param name="listaActividades">Lista de actividades disponibles.</param>
        /// <param name="listaReservas">Lista de reservas existentes.</param>
        /// <param name="error">Mensaje de error devuelto si no es valido.</param>
        /// <param name="reservaIdExcluida">Id de una reserva a excluir en la comprobacion (opcional, para modificaciones).</param>
        /// <returns>True si la reserva es valida, false en caso contrario.</returns>
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
        /// <summary>
        /// Valida los datos del formulario de actividad y devuelve el aforo parseado.
        /// </summary>
        /// <param name="nombre">Nombre de la actividad.</param>
        /// <param name="aforoMaxTexto">Texto con el aforo maximo a validar.</param>
        /// <param name="error">Mensaje de error devuelto si no es valido.</param>
        /// <param name="aforo">Salida con el aforo valido convertido a entero.</param>
        /// <returns>True si el formulario es valido, false en caso contrario.</returns>
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
