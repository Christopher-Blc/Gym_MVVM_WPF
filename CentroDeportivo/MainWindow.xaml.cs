using System.Windows;

namespace CentroDeportivo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
    }
}
