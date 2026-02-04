using CrystalDecisions.CrystalReports.Engine;
using Informes;

namespace Centro_ViewModel
{
    /// <summary>
    /// ViewModel para Informe2. Crea el ReportDocument filtrado por una actividad.
    /// </summary>
    public class Informe2ViewModel
    {
        /// <summary>
        /// ReportDocument listo para enlazar con el control de informe.
        /// </summary>
        public ReportDocument Informe { get; }

        /// <summary>
        /// Constructor. Obtiene los datos y crea el informe para la actividad especificada.
        /// </summary>
        /// <param name="idActividad">Id de la actividad para filtrar el informe.</param>
        public Informe2ViewModel(int idActividad)
        {
            //datos
            var helper = new Helper();
            var ds = helper.DatosInforme2(idActividad);

            //report
            var rpt = new CRInforme2(); 
            rpt.SetDataSource(ds);

            //parametro crystal 
            rpt.SetParameterValue("IdActividad", idActividad);

            Informe = rpt;
        }
    }
}
