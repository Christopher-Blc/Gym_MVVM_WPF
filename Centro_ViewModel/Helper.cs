using Centro_ViewModel;
using Centro_Model;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centro_ViewModel
{
    public class Helper
    {
        
        private string conexion = "Server=CHRIS-PC\\SQLEXPRESS;Database=CentroDeportivo;Trusted_Connection=True;";
        public DsInformes1 DatosInforme1()
        {
            var ds = new DsInformes1();

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

        public DsInforme2 DatosInforme2(int idActividad)
        {
            var ds = new DsInforme2();

            string sql = @"
                SELECT 
                    a.Nombre AS NombreActividad,
                    r.Fecha AS FechaReserva,
                    s.Nombre AS NombreSocio,
                    a.AforoMaximo
                FROM Reservas r
                JOIN Socios s ON r.SocioId = s.Id
                JOIN Actividades a ON r.ActividadId = a.Id
                WHERE a.Id = @IdActividad
                ORDER BY r.Fecha
            ";

            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@IdActividad", idActividad);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ds.DTInforme2.Rows.Add(
                            dr["NombreActividad"],
                            dr["FechaReserva"],
                            dr["NombreSocio"],
                            dr["AforoMaximo"]
                        );
                    }
                }
            }

            return ds;
        }

        public List<Actividades> GetActividades()
        {
            var lista = new List<Actividades>();

            string sql = "SELECT Id, Nombre FROM Actividades";

            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Actividades
                        {
                            Id = (int)dr["Id"],
                            Nombre = dr["Nombre"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public DsInforme3 DatosInforme3()
        {
            var ds = new DsInforme3();

            string sql = @"
                SELECT 
                    s.Nombre AS NombreSocio,
                    a.Nombre AS NombreActividad,
                    r.Fecha  AS FechaReserva
                FROM Reservas r
                JOIN Socios s ON s.Id = r.SocioId
                JOIN Actividades a ON a.Id = r.ActividadId
                ORDER BY s.Nombre, r.Fecha
            ";

            using (SqlConnection cn = new SqlConnection(conexion))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ds.DTInforme3.Rows.Add(
                            dr["NombreSocio"],
                            dr["NombreActividad"],
                            dr["FechaReserva"]
                        );
                    }
                }
            }

            return ds;
        }


    }




}
