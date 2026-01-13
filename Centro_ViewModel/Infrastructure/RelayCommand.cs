using System;
using System.Windows.Input;

namespace Centro_ViewModel.Infrastructure
{
    public class RelayCommand : ICommand
    {
        private readonly Action ejecutar;
        private readonly Func<bool> puedeEjecutar;

        public RelayCommand(Action ejecutar, Func<bool> puedeEjecutar = null)
        {
            this.ejecutar = ejecutar;
            this.puedeEjecutar = puedeEjecutar;
        }

        public bool CanExecute(object parameter) => puedeEjecutar == null || puedeEjecutar();

        public void Execute(object parameter) => ejecutar();

        public event EventHandler CanExecuteChanged;

        public void NotificarPuedeEjecutar() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
