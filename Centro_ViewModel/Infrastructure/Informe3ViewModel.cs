using Centro_Model;
using CrystalDecisions.CrystalReports.Engine;
using Informes;

namespace Centro_ViewModel
{
    public class Informe3ViewModel
    {
        public ReportDocument Informe { get; }

        public Informe3ViewModel()
        {
            var helper = new Helper();
            var ds = helper.DatosInforme3();

            var rpt = new CRInforme3();
            rpt.SetDataSource(ds);

            Informe = rpt;
        }
    }
}
