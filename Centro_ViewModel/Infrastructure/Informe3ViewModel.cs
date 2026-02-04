using Centro_Model;
using CrystalDecisions.CrystalReports.Engine;
using Informes;

namespace Centro_ViewModel
{
    /// <summary>
    /// ViewModel para Informe3. Crea el ReportDocument con los datos necesarios.
    /// </summary>
    public class Informe3ViewModel
    {
        /// <summary>
        /// ReportDocument listo para enlazar con el control de informe.
        /// </summary>
        public ReportDocument Informe { get; }

        /// <summary>
        /// Constructor. Obtiene los datos y crea el informe.
        /// </summary>
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
