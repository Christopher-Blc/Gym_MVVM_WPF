using Centro_ViewModel;
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
    /// Interaction logic for WindowInforme3.xaml
    /// </summary>
    public partial class WindowInforme3 : Window
    {
        public WindowInforme3()
        {
            InitializeComponent();

            var vm = new Informe3ViewModel();
            DataContext = vm;

            reportViewer.ViewerCore.ReportSource = vm.Informe;
            reportViewer.Owner = this;
        }

    }
}
