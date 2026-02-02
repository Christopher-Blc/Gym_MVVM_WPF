using CrystalDecisions.CrystalReports.Engine;
using Informes;

namespace Centro_ViewModel
{
    public class Informe2ViewModel
    {
        public ReportDocument Informe { get; }



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
