using Centro_Model;
using Centro_ViewModel.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace Centro_ViewModel.Infrastructure
{
    /// <summary>
    /// ViewModel que se encarga de las actividades del programa
    /// Nos da la lista de actividades, la actividad seleccionada, los campos del formulario
    /// y los comandos para anyadir, modificar y eliminar actividades.
    /// </summary>
    public class ActividadesViewModel : BaseViewModel
    {
        /// <summary>
        /// lista de las actividades cargadas desde la bbdd.
        /// </summary>
        private List<Actividades> listaActividades;

        /// <summary>
        /// Lista de actividades cargadas desde la bbdd.
        /// Se notifica la vista cuando cambia la lista para actualizar los bindings.
        /// </summary>
        public List<Actividades> ListaActividades
        {
            get => listaActividades;
            set
            {
                listaActividades = value;
                OnPropertyChanged(nameof(ListaActividades));
            }
        }

        /// <summary>
        /// Actividad seleccionada en la vista.
        /// cuando se selecciona una actividad se cargan los campos del formulario (Nombre y AforoMax).
        /// </summary>
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

        /// <summary>
        /// Valor del campo Nombre del formulario.
        /// Usado para crear o modificar una actividad.
        /// </summary>
        private string nombre;
        public string Nombre
        {
            get => nombre;
            set { nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }

        /// <summary>
        /// Valor del campo Aforo max del formulario.
        /// Se valida y convierte antes de subirse en la base de datos.
        /// </summary>
        private string aforoMax;
        public string AforoMax
        {
            get => aforoMax;
            set { aforoMax = value; OnPropertyChanged(nameof(AforoMax)); }
        }


        // Commands que creamos para vincular la accion del boton de la view con la accion definida aqui

        /// <summary>
        /// Comando para anyadir una nueva actividad.
        /// </summary>
        public ICommand AnyadirCommand { get; }

        /// <summary>
        /// Comando para modificar la actividad seleccionada.
        /// </summary>
        public ICommand ModificarCommand { get; }

        /// <summary>
        /// Comando para eliminar la actividad seleccionada.
        /// </summary>
        public ICommand EliminarCommand { get; }

        /// <summary>
        /// Constructor que inicializa los comandos y carga la lista de actividades.
        /// </summary>
        public ActividadesViewModel()
        {
            // Creamos aqui los commands y le pasamos el metodo que ejecutara
            AnyadirCommand = new RelayCommand(Anyadir);
            ModificarCommand = new RelayCommand(Modificar);
            EliminarCommand = new RelayCommand(Eliminar);
            Recargar();
        }

        /// <summary>
        /// Carga las actividades desde la base de datos y actualiza la lista de actividades.
        /// </summary>
        /// <returns>void</returns>
        private void Recargar()
        {
            // para cargar las actividades de la BBDD en la lista 
            using (var contexto = new CentroDeportivoEntities())
            {
                ListaActividades = contexto.Actividades.ToList();
            }
        }

        /// <summary>
        /// Valida los campos del formulario y anyade una nueva actividad a la base de datos.
        /// Al finalizar recarga la lista y limpia el formulario.
        /// </summary>
        /// <returns>void</returns>
        private void Anyadir()
        {
            if (!Validaciones.ActividadFormularioValido(Nombre, AforoMax, out var error, out var aforo))
            {
                MessageBox.Show(error);
                return;
            }

            using (var contexto = new CentroDeportivoEntities())
            {
                var nueva = new Actividades
                {
                    Nombre = Validaciones.LimpiarTexto(Nombre),
                    AforoMaximo = aforo
                };

                contexto.Actividades.Add(nueva);
                contexto.SaveChanges();
            }

            Recargar();
            LimpiarFormulario();
        }


        /// <summary>
        /// Modifica la actividad seleccionada con los valores del formulario.
        /// Valida los datos y persiste los cambios en la base de datos.
        /// </summary>
        /// <returns>void</returns>
        private void Modificar()
        {
            if (ActividadSeleccionada == null)
            {
                MessageBox.Show("Ninguna actividad seleccionada");
                return;
            }

            if (!Validaciones.ActividadFormularioValido(Nombre, AforoMax, out var error, out var aforo))
            {
                MessageBox.Show(error);
                return;
            }

            using (var contexto = new CentroDeportivoEntities())
            {
                var act = contexto.Actividades.Find(ActividadSeleccionada.Id);
                if (act == null) return;

                act.Nombre = Validaciones.LimpiarTexto(Nombre);
                act.AforoMaximo = aforo;
                contexto.SaveChanges();
            }

            Recargar();
            LimpiarFormulario();
        }


        /// <summary>
        /// Elimina la actividad seleccionada de la base de datos.
        /// cuando acaba,  recarga la lista y limpia el formulario.
        /// </summary>
        /// <returns>void</returns>
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


        /// <summary>
        /// Limpia los campos del formulario y desselecciona la actividad.
        /// </summary>
        /// <returns>void</returns>
        private void LimpiarFormulario()
        {
            Nombre = "";
            AforoMax = "";
            ActividadSeleccionada = null;
        }
    }
}
