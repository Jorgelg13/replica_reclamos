using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ReplicaDataReclamos
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                string basePath = @"C:\FTP\Guatemala\Reclamos\Cabina\";
                ExportReclamos(getData(Consultas.RECLAMOS_AUTOS()),basePath + "reclamos_autos.xlsx","reclamo_auto");
                ExportReclamos(getData(Consultas.RECLAMOS_VARIOS()),basePath + "reclamos_varios.xlsx","reclamos_varios");
                ExportReclamos(getData(Consultas.RECLAMOS_MEDICOS()),basePath + "reclamos_medicos.xlsx","reclamos_medicos");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static DataTable getData(string query)
        {
            try
            {
                DataTable dataTable = new DataTable();

                using (SqlConnection connection = new SqlConnection(Consultas.CADENA_CONEXION()))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                        {
                            dataAdapter.Fill(dataTable);
                        }
                    }

                    connection.Close();
                }

                return dataTable;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        static void ExportReclamos(DataTable reclamos, string filePath, string tipo)
        {
            DataTable bitacora = null;
            List<int> ids = new List<int>();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("reclamos");

                for (int i = 0; i < reclamos.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = reclamos.Columns[i].ColumnName;
                }

                for (int i = 0; i < reclamos.Rows.Count; i++)
                {
                    for (int j = 0; j < reclamos.Columns.Count; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = reclamos.Rows[i][j].ToString();

                        if (j == 0)
                        {
                            ids.Add(Convert.ToInt32(reclamos.Rows[i][j]));
                        }
                    }
                }

                if(tipo == "reclamo_auto" && ids.Count > 0)
                {
                    bitacora = getData(Consultas.BITACORA_AUTOS(ids));
                } 
                
                else if(tipo == "reclamos_varios" && ids.Count > 0)
                {
                    bitacora = getData(Consultas.BITACORA_RECLAMOS_VARIOS(ids));
                } 
                
                else if(tipo == "reclamos_medicos" && ids.Count > 0)
                {
                    bitacora = getData(Consultas.RECIBOS_MEDICOS(ids));
                }

                var worksheetTwo = workbook.Worksheets.Add("bitacoras");

                if (ids.Count > 0)
                {
                    for (int i = 0; i < bitacora.Columns.Count; i++)
                    {
                        worksheetTwo.Cell(1, i + 1).Value = bitacora.Columns[i].ColumnName;
                    }

                    for (int i = 0; i < bitacora.Rows.Count; i++)
                    {
                        for (int j = 0; j < bitacora.Columns.Count; j++)
                        {
                            worksheetTwo.Cell(i + 2, j + 1).Value = bitacora.Rows[i][j].ToString();
                        }
                    }
                }

                workbook.SaveAs(filePath);

                if (ids.Count >0)
                {
                    updateRegistros(tipo, ids);
                }
            }
        }

        static void updateRegistros(string tabla, List<int> ids)
        {
            string query = "UPDATE " + tabla + " SET replica_ibis = 1 WHERE id in (" + string.Join(",", ids) + ")";

            try
            {
                using (SqlConnection connection = new SqlConnection(Consultas.CADENA_CONEXION()))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int filasActualizadas = command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar el registro: " + ex.Message);
            }
        }
    }
}
