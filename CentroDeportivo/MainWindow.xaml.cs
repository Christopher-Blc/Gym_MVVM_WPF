using Centro_ViewModel;
using System.Windows;

namespace CentroDeportivo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

        }

        private void BtnSocios_Click(object sender, RoutedEventArgs e)
        {
            var w = new WindowSocios();
            w.Show();
        }

        private void BtnReservas_Click(object sender, RoutedEventArgs e)
        {
            var w = new WindowReservas();
            w.Show();
        }

        private void BtnActividades_Click(object sender, RoutedEventArgs e)
        {
            var w = new WindowActividades();
            w.Show();
        }

        private void BtnInforme1_Click(object sender, RoutedEventArgs e)
        {
           
            var w = new WindowInforme1();
            w.Show();
            
        }

        private void BtnInforme2_Click(object sender, RoutedEventArgs e)
        {
            //hacia falta para comporbar que haya actividad seleccionada 
            var vm = (MainWindowViewModel)DataContext;

            if (vm.ActividadSeleccionadaId == null)
            {
                MessageBox.Show("Selecciona una actividad");
                return;
            }

            new WindowInforme2(vm.ActividadSeleccionadaId.Value).Show();
        }

        private void BtnInforme3_Click(object sender, RoutedEventArgs e)
        {
            var w = new WindowInforme3();
            w.Show();
        }
    }
}
