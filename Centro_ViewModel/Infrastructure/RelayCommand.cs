using System;
using System.Windows.Input;

namespace Centro_ViewModel.Infrastructure
{
    /// <summary>
    /// Implementacion de ICommand que ejecuta una accion y opcionalmente
    /// pregunta si puede ejecutarse mediante un delegado.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action ejecutar;
        private readonly Func<bool> puedeEjecutar;

        /// <summary>
        /// Crea una nueva instancia de RelayCommand.
        /// </summary>
        /// <param name="ejecutar">Accion a ejecutar cuando se invoque el comando.</param>
        /// <param name="puedeEjecutar">Funcion que determina si el comando puede ejecutarse. Puede ser null.</param>
        public RelayCommand(Action ejecutar, Func<bool> puedeEjecutar = null)
        {
            this.ejecutar = ejecutar;
            this.puedeEjecutar = puedeEjecutar;
        }

        /// <summary>
        /// Determina si el comando puede ejecutarse en el estado actual.
        /// </summary>
        /// <param name="parameter">Parametro pasado por la vista (no usado en esta implementacion).</param>
        /// <returns>True si se puede ejecutar, false en caso contrario.</returns>
        public bool CanExecute(object parameter) => puedeEjecutar == null || puedeEjecutar();

        /// <summary>
        /// Ejecuta la accion asociada al comando.
        /// </summary>
        /// <param name="parameter">Parametro pasado por la vista (no usado en esta implementacion).</param>
        public void Execute(object parameter) => ejecutar();

        /// <summary>
        /// Evento que se dispara cuando cambia la posibilidad de ejecutar el comando.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Notifica a los suscriptores que el estado de ejecucion del comando puede haber cambiado.
        /// </summary>
        /// <returns>void</returns>
        public void NotificarPuedeEjecutar() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
