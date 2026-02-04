using Centro_Model;
using Centro_ViewModel.Infrastructure;
using System.Collections.Generic;
using System.Windows.Input;

namespace Centro_ViewModel
{
    /// <summary>
    /// ViewModel para la ventana principal. Proporciona la lista de actividades y
    /// mantiene la id de la actividad seleccionada.
    /// </summary>
    public class MainWindowViewModel
    {
        /// <summary>
        /// Lista de actividades para mostrar en la ventana principal.
        /// </summary>
        public List<Actividades> Actividades { get; }

        /// <summary>
        /// Id de la actividad seleccionada en la vista principal (nullable).
        /// </summary>
        public int? ActividadSeleccionadaId { get; set; }

        /// <summary>
        /// Constructor. Carga la lista de actividades usando el helper.
        /// </summary>
        public MainWindowViewModel()
        {
            var helper = new Helper();
            Actividades = helper.GetActividades();
        }
    }
}
