using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CentroDeportivo
{
    /// <summary>
    /// Interaction logic for WindowSocios.xaml
    /// </summary>
    public partial class WindowSocios : Window
    {
        public WindowSocios()
        {
            InitializeComponent();
            DataContext = new Centro_ViewModel.Infrastructure.SociosViewModel();
        }
    }
}
