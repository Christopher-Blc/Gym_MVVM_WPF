using System.Windows;
using Centro_ViewModel;

namespace CentroDeportivo
{
    public partial class WindowInforme2 : Window
    {
        public WindowInforme2(int idActividad)
        {
            InitializeComponent();

            var vm = new Informe2ViewModel(idActividad);
            DataContext = vm;

            reportViewer.ViewerCore.ReportSource = vm.Informe;
            reportViewer.Owner = this;
        }

        private void reportViewer_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
