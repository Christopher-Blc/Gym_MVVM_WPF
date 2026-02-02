using CrystalDecisions.CrystalReports.Engine;
using Informes;

namespace Centro_ViewModel
{
    public class Informe1ViewModel
    {
        public CrystalDecisions.CrystalReports.Engine.ReportDocument Informe { get; }


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
