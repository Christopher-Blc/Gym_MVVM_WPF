using Microsoft.VisualStudio.TestTools.UnitTesting;
using Centro_ViewModel.Infrastructure;
using Centro_Model;
using System;
using System.Collections.Generic;
using Centro_ViewModel;


namespace Centro_Tests
{
    [TestClass]
    public class ValidacionesTests
    {
        [TestMethod]
        public void ValidacionFormatoEmail_DebeDetectarEmailsValidosEInvalidos()
        {
            string error;

            // valido
            var ok = Validaciones.EmailValido("usuario@dominio.com", out error);
            Assert.IsTrue(ok);

            // invalido
            var bad = Validaciones.EmailValido("usuario.com", out error);
            Assert.IsFalse(bad);

            // invalido
            bad = Validaciones.EmailValido("usuario@dominio", out error);
            Assert.IsFalse(bad);

            // invalido
            bad = Validaciones.EmailValido(" ", out error);
            Assert.IsFalse(bad);
        }

        [TestMethod]
        public void ValidacionFechaReserva_NoPermiteFechaAnteriorAHoy()
        {
            var sociosId = 1;
            var actividadId = 10;

            var listaActividades = new List<Actividades>
            {
                new Actividades { Id = actividadId, Nombre = "Spinning", AforoMaximo = 10 }
            };

            var listaReservas = new List<Reservas>();

            var ayer = DateTime.Today.AddDays(-1);

            var ok = Validaciones.ReservaFormularioValido(
                sociosId,
                actividadId,
                ayer,
                listaActividades,
                listaReservas,
                out var error
            );

            Assert.IsFalse(ok);
            Assert.AreEqual("La fecha no puede ser anterior a hoy.", error);
        }

        [TestMethod]
        public void ControlAforoMaximo_NoPermiteSuperarAforo()
        {
            var socio1Id = 1;
            var socio2Id = 2;
            var actividadId = 20;

            // actividad con aforo 1
            var listaActividades = new List<Actividades>
            {
                new Actividades { Id = actividadId, Nombre = "Pilates", AforoMaximo = 1 }
            };

            var hoy = DateTime.Today;

            // primera reserva ya existente en la lista, simula que ya hay 1 plaza ocupada
            var listaReservas = new List<Reservas>
            {
                new Reservas { Id = 100, SocioId = socio1Id, ActividadId = actividadId, Fecha = hoy }
            };

            // intento de crear segunda reserva para la misma actividad
            var ok = Validaciones.ReservaFormularioValido(
                socio2Id,
                actividadId,
                hoy,
                listaActividades,
                listaReservas,
                out var error
            );

            Assert.IsFalse(ok);
            Assert.AreEqual("El aforo maximo ya se ha llenado", error);
        }
    }
}
