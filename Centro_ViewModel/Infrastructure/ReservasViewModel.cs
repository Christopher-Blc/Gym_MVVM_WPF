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
    public class ReservasViewModel : BaseViewModel
    {



        //Listado de las reservas
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

        //obtener la reserva seleccionada del datagrid en la view
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


        //guardan el id
        private int socioSeleccionado;
        public int SocioSeleccionado
        {
            get => socioSeleccionado;
            set { socioSeleccionado = value; OnPropertyChanged(nameof(SocioSeleccionado)); }
        }

        
        private int actividadSeleccionada;
        public int ActividadSeleccionada
        {
            get => actividadSeleccionada;
            set { actividadSeleccionada = value; OnPropertyChanged(nameof(ActividadSeleccionada)); }
        }

        private DateTime fechaSeleccionada;
        public DateTime FechaSeleccionada
        {
            get => fechaSeleccionada;
            set { fechaSeleccionada = value; OnPropertyChanged(nameof(FechaSeleccionada)); }
        }

        //binding de listas 
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


        //Commands que creamos para vincular la accion del boton de la view con la accion definida aqui
        public ICommand AnyadirCommand { get; }
        public ICommand ModificarCommand { get; }
        public ICommand EliminarCommand { get; }

        public ReservasViewModel()
        {
            //Creamos aqui los comands y le pasamos el metodo que ejecutara
            AnyadirCommand = new RelayCommand(Anyadir);
            ModificarCommand = new RelayCommand(Modificar);
            EliminarCommand = new RelayCommand(Eliminar);
            Recargar();
        }

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

        //Metodo de añadir que se pasa despues al command
        private void Anyadir()
        {
            var actividad = ListaActividades.FirstOrDefault(a => a.Id == ActividadSeleccionada);
            if (SocioSeleccionado == -1)
            {
                MessageBox.Show("Elige un socio");
                return;
            }
            if (actividad == null)
            {
                MessageBox.Show("Elige una actividad");
                return;
            }
            if (FechaSeleccionada < DateTime.Now)
            {
                MessageBox.Show("La fecha no puede ser anterior a hoy.");
                return;
            }

            int cantidadReservas = 0;
            foreach (var item in ListaReservas)
            {
                if (item.ActividadId == ActividadSeleccionada)
                {
                    cantidadReservas++;
                }
            }
            if (cantidadReservas >= actividad.AforoMaximo)
            {
                MessageBox.Show("El aforo macimo ya se ha llenado");
                return;
            }

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

        private void Modificar()
        {

            if (ReservaSeleccionada == null)
            {
                MessageBox.Show("Por favor , seleccione una reserva.");
                return;
            }
            if (SocioSeleccionado == -1)
            {
                MessageBox.Show("Elige un socio");
                return;
            }
            if (ActividadSeleccionada == -1)
            {
                MessageBox.Show("Elige una actividad");
                return;
            }
            var actividad = ListaActividades.FirstOrDefault(a => a.Id == ActividadSeleccionada);
            if (FechaSeleccionada < DateTime.Now)
            {
                MessageBox.Show("La fecha no puede ser anterior a hoy.");
                return;
            }

            int cantidadReservas = 0;
            foreach (var item in ListaReservas)
            {
                if (item.ActividadId == ActividadSeleccionada && item.Id != ReservaSeleccionada.Id)
                {
                    cantidadReservas++;
                }
            }
            if (cantidadReservas >= actividad.AforoMaximo)
            {
                MessageBox.Show("El aforo macimo ya se ha llenado");
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

        private void LimpiarFormulario()
        {
            ActividadSeleccionada = -1;
            SocioSeleccionado = -1;
            FechaSeleccionada = DateTime.Now;
            ReservaSeleccionada = null;
        }
    }
}
