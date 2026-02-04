using CrystalDecisions.CrystalReports.Engine;
using Informes;

namespace Centro_ViewModel
{
    /// <summary>
    /// ViewModel para Informe1, crea el ReportDocument con los datos que hacen falta.
    /// </summary>
    public class Informe1ViewModel
    {
        /// <summary>
        /// ReportDocument listo para conectar con el control de la ventana.
        /// </summary>
        public CrystalDecisions.CrystalReports.Engine.ReportDocument Informe { get; }

        /// <summary>
        /// Constructor que obtiene los datos y crea el informe
        /// </summary>
        public Informe1ViewModel()
        {
            //obtener datos
            var helper = new Helper();
            var ds = helper.DatosInforme1();

            //crear informe (clase generada por el .rpt)
            var rpt = new CRInforme1();  
            rpt.SetDataSource(ds);

            Informe = rpt;
        }
    }
}
