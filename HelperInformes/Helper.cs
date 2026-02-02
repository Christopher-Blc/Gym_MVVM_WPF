using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperInformes
{
    public class Helper
    {
        public DsInformes DatosInforme1()
        {
            var ds = new DsInformes();

            string conexion = "Server=CHRIS-PC\\SQLEXPRESS;Database=master;Trusted_Connection=True;";
            string sql = "SELECT s.Id , s.Nombre , s.Email , s.Activo FROM Socios s";

            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ds.DTSocios.Rows.Add(
                        dr["Id"],
                        dr["Nombre"],
                        dr["Email"],
                        dr["Activo"]
                    );

                }
                return ds;

            }

        }
    }

           

    
}
