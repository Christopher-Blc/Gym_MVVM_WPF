using Centro_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Centro_ViewModel.Infrastructure
{
    /// <summary>
    /// ViewModel encargado de la gestion de reservas.
    /// Proporciona la lista de reservas, la reserva seleccionada, los campos del formulario
    /// y los comandos para anyadir, modificar y eliminar reservas.
    /// </summary>
    public class ReservasViewModel : BaseViewModel
    {

        /// <summary>
        /// Listado de las reservas.
        /// </summary>
        private List<Reservas> listaReservas;
        public List<Reservas> ListaReservas
        {
            get => listaReservas;
            set
            {
                listaReservas = value;
                OnPropertyChanged(nameof(ListaReservas));
            }
        }

        /// <summary>
        /// Reserva seleccionada en la vista.
        /// Al establecerla se cargan los campos del formulario.
        /// </summary>
        private Reservas reservaSeleccionada;
        public Reservas ReservaSeleccionada
        {
            get => reservaSeleccionada;
            set
            {
                reservaSeleccionada = value;
                OnPropertyChanged(nameof(ReservaSeleccionada));

                // Al seleccionar fila, cargamos los TextBox
                if (reservaSeleccionada != null)
                {
                    SocioSeleccionado = reservaSeleccionada.SocioId;
                    ActividadSeleccionada = reservaSeleccionada.ActividadId;
                    FechaSeleccionada = reservaSeleccionada.Fecha;

                }
            }
        }


        /// <summary>
        /// Id del socio seleccionado en el formulario.
        /// </summary>
        private int socioSeleccionado;
        public int SocioSeleccionado
        {
            get => socioSeleccionado;
            set { socioSeleccionado = value; OnPropertyChanged(nameof(SocioSeleccionado)); }
        }

        /// <summary>
        /// Id de la actividad seleccionada en el formulario.
        /// </summary>
        private int actividadSeleccionada;
        public int ActividadSeleccionada
        {
            get => actividadSeleccionada;
            set { actividadSeleccionada = value; OnPropertyChanged(nameof(ActividadSeleccionada)); }
        }

        /// <summary>
        /// Fecha seleccionada en el formulario.
        /// </summary>
        private DateTime fechaSeleccionada;
        public DateTime FechaSeleccionada
        {
            get => fechaSeleccionada;
            set { fechaSeleccionada = value; OnPropertyChanged(nameof(FechaSeleccionada)); }
        }

        /// <summary>
        /// Lista de socios para binding en la vista.
        /// </summary>
        private List<Socios> listaSocios;
        public List<Socios> ListaSocios
        {
            get => listaSocios;
            set
            {
                listaSocios = value;
                OnPropertyChanged(nameof(ListaSocios));
            }
        }

        /// <summary>
        /// Lista de actividades para binding en la vista.
        /// </summary>
        private List<Actividades> listaActividades;
        public List<Actividades> ListaActividades
        {
            get => listaActividades;
            set
            {
                listaActividades = value;
                OnPropertyChanged(nameof(ListaActividades));
            }
        }


        // Commands que creamos para vincular la accion del boton de la view con la accion definida aqui

        /// <summary>
        /// Comando para anyadir una nueva reserva.
        /// </summary>
        public ICommand AnyadirCommand { get; }
        /// <summary>
        /// Comando para modificar una reserva.
        /// </summary>
        public ICommand ModificarCommand { get; }
        /// <summary>
        /// Comando para eliminar una reserva.
        /// </summary>
        public ICommand EliminarCommand { get; }

        /// <summary>
        /// Constructor. Inicializa los comandos y recarga los datos.
        /// </summary>
        public ReservasViewModel()
        {
            //Creamos aqui los comands y le pasamos el metodo que ejecutara
            AnyadirCommand = new RelayCommand(Anyadir);
            ModificarCommand = new RelayCommand(Modificar);
            EliminarCommand = new RelayCommand(Eliminar);
            Recargar();
        }

        /// <summary>
        /// Carga las reservas, socios y actividades desde la base de datos.
        /// </summary>
        /// <returns>void</returns>
        private void Recargar()
        {
            //para cargar las reservas de la BBDD en la lista 
            using (var contexto = new CentroDeportivoEntities())
            {
                ListaReservas = contexto.Reservas.ToList();
                ListaSocios = contexto.Socios.ToList();
                ListaActividades = contexto.Actividades.ToList();

            }
        }

        /// <summary>
        /// Valida los campos del formulario y anyade una nueva reserva a la base de datos.
        /// Al finalizar recarga la lista y limpia el formulario.
        /// </summary>
        /// <returns>void</returns>
        private void Anyadir()
        {
            //enviamos los datos al metodo que valida y cogemos el error que devuelve si esque haya uno
            if (!Validaciones.ReservaFormularioValido(
             SocioSeleccionado,
             ActividadSeleccionada,
             FechaSeleccionada,
             ListaActividades,
             ListaReservas,
             out var error
             ))
            {
                MessageBox.Show(error);
                return;
            }

            //si no hay error , nos crea la reserva
            using (var contexto = new CentroDeportivoEntities())
            {
                var nueva = new Reservas
                {
                    SocioId = SocioSeleccionado,
                    ActividadId = ActividadSeleccionada,
                    Fecha = FechaSeleccionada,
                };

                contexto.Reservas.Add(nueva);
                contexto.SaveChanges();
            }

            Recargar();
            LimpiarFormulario();
        }

        /// <summary>
        /// Modifica la reserva seleccionada con los valores del formulario.
        /// Valida los datos y persiste los cambios en la base de datos.
        /// </summary>
        /// <returns>void</returns>
        private void Modificar()
        {

            if (ReservaSeleccionada == null)
            {
                MessageBox.Show("Por favor , seleccione una reserva.");
                return;
            }

            if (!Validaciones.ReservaFormularioValido(
                    SocioSeleccionado,
                    ActividadSeleccionada,
                    FechaSeleccionada,
                    ListaActividades,
                    ListaReservas,
                    out var error,
                    reservaIdExcluida: ReservaSeleccionada.Id
                ))
            {
                MessageBox.Show(error);
                return;
            }

            using (var contexto = new CentroDeportivoEntities())
            {
                var reserva = contexto.Reservas.Find(ReservaSeleccionada.Id);
                if (reserva == null) return;

                reserva.SocioId = SocioSeleccionado;
                reserva.ActividadId = ActividadSeleccionada;
                reserva.Fecha = FechaSeleccionada;
                contexto.SaveChanges();
            }

            Recargar();
            LimpiarFormulario();
        }

        /// <summary>
        /// Elimina la reserva seleccionada de la base de datos.
        /// </summary>
        /// <returns>void</returns>
        private void Eliminar()
        {
            if (ReservaSeleccionada == null)
            {
                MessageBox.Show("Ninguna reserva seleccionada");
                return;
            }

            using (var contexto = new CentroDeportivoEntities())
            {
                var res = contexto.Reservas.Find(ReservaSeleccionada.Id);
                if (res == null) return;

                contexto.Reservas.Remove(res);
                contexto.SaveChanges();
            }

            Recargar();
            LimpiarFormulario();
        }

        /// <summary>
        /// Limpia los campos del formulario y deselecciona la reserva.
        /// </summary>
        /// <returns>void</returns>
        private void LimpiarFormulario()
        {
            ActividadSeleccionada = -1;
            SocioSeleccionado = -1;
            FechaSeleccionada = DateTime.Now;
            ReservaSeleccionada = null;
        }
    }
}
