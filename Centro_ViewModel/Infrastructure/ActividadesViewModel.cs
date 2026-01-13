using Centro_Model;
using Centro_ViewModel.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace Centro_ViewModel.ViewModels
{
    public class ActividadesViewModel : BaseViewModel
    {
        //Listado de las actividades 
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

        //obtener la actividad seleccionada del datagrid en la view
        private Actividades actividadSeleccionada;
        public Actividades ActividadSeleccionada
        {
            get => actividadSeleccionada;
            set
            {
                actividadSeleccionada = value;
                OnPropertyChanged(nameof(ActividadSeleccionada));

                // Al seleccionar fila, cargamos los TextBox
                if (actividadSeleccionada != null)
                {
                    Nombre = actividadSeleccionada.Nombre;
                    AforoMax = actividadSeleccionada.AforoMaximo.ToString();
                }
            }
        }

        //Para obtener el valor de los campos de la view (nombre y aforoMax)
        private string nombre;
        public string Nombre
        {
            get => nombre;
            set { nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }

        private string aforoMax;
        public string AforoMax
        {
            get => aforoMax;
            set { aforoMax = value; OnPropertyChanged(nameof(AforoMax)); }
        }


        //Commands que creamos para vincular la accion del boton de la view con la accion definida aqui
        public ICommand AnyadirCommand { get; }
        public ICommand ModificarCommand { get; }
        public ICommand EliminarCommand { get; }

        public ActividadesViewModel()
        {
            //Creamos aqui los comands y le pasamos el metodo que ejecutara
            AnyadirCommand = new RelayCommand(Anyadir);
            ModificarCommand = new RelayCommand(Modificar);
            EliminarCommand = new RelayCommand(Eliminar);
            Recargar();
        }

        private void Recargar()
        {
            //para cargar las actividades de la BBDD en la lista 
            using (var contexto = new CentroDeportivoEntities())
            {
                ListaActividades = contexto.Actividades.ToList();
            }
        }

        //Metodo de añadir que se pasa despues al command
        private void Anyadir()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MessageBox.Show("El nombre no puede estar vacío");
                return;
            }
            if (!int.TryParse(AforoMax, out int aforo) || aforo <= 0)
            {
                MessageBox.Show("El aforo tiene que ser un numero positivo");
                return;
            }

            using (var contexto = new CentroDeportivoEntities())
            {
                var nueva = new Actividades
                {
                    Nombre = Nombre.Trim(),
                    AforoMaximo = aforo
                };

                contexto.Actividades.Add(nueva);
                contexto.SaveChanges();
            }

            Recargar();
            LimpiarFormulario();
        }


        private void Modificar()
        {
            if (ActividadSeleccionada == null)
            {
                MessageBox.Show("Ninguna actividad seleccionada");
                return;
            }
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MessageBox.Show("El nombre no puede estar vacio");
                return;
            }
            if (!int.TryParse(AforoMax, out int aforo) || aforo <= 0)
            {
                MessageBox.Show("El aforo tiene que ser un numero positivo");
                return;
            }

            using (var contexto = new CentroDeportivoEntities())
            {
                var act = contexto.Actividades.Find(ActividadSeleccionada.Id);
                if (act == null) return;

                act.Nombre = Nombre.Trim();
                act.AforoMaximo = aforo;
                contexto.SaveChanges();
            }

            Recargar();
            LimpiarFormulario();
        }


        private void Eliminar()
        {
            if (ActividadSeleccionada == null)
            {
                MessageBox.Show("Ninguna actividad seleccionada");
                return;
            }

            using (var contexto = new CentroDeportivoEntities())
            {
                var act = contexto.Actividades.Find(ActividadSeleccionada.Id);
                if (act == null) return;

                contexto.Actividades.Remove(act);
                contexto.SaveChanges();
            }

            Recargar();
            LimpiarFormulario();
        }


        private void LimpiarFormulario()
        {
            Nombre = "";
            AforoMax = "";
            ActividadSeleccionada = null;
        }
    }
}
