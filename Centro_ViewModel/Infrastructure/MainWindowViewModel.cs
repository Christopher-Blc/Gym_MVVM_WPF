using Centro_Model;
using Centro_ViewModel.Infrastructure;
using System.Collections.Generic;
using System.Windows.Input;

namespace Centro_ViewModel
{
    public class MainWindowViewModel
    {
        public List<Actividades> Actividades { get; }
        public int? ActividadSeleccionadaId { get; set; }

        public MainWindowViewModel()
        {
            var helper = new Helper();
            Actividades = helper.GetActividades();
        }
    }
}
