using System;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;

namespace ReplicaDataReclamos
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                string basePath = @"C:\FTP\Guatemala\Reclamos\Cabina\";
                ExportReclamos(
                    getData(Consultas.RECLAMOS_AUTOS()),
                    getData(Consultas.BITACORA_AUTOS()),
                    basePath + "reclamos_autos.xlsx");

                ExportReclamos(
                    getData(Consultas.RECLAMOS_VARIOS()),
                    getData(Consultas.BITACORA_RECLAMOS_VARIOS()),
                    basePath + "reclamos_varios.xlsx");

                ExportReclamos(
                    getData(Consultas.RECLAMOS_MEDICOS()),
                    getData(Consultas.RECIBOS_MEDICOS()),
                    basePath + "reclamos_medicos.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static DataTable getData(string query)
        {
            string connectionString = "Persist Security Info=False;User ID=sa;Password=admin123;Initial Catalog=reclamos;Server=alienware\\SQLEXPRESS";

            try
            {
                DataTable dataTable = new DataTable();

                using (SqlConnection connection = new SqlConnection(connectionString))
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

        static void ExportReclamos(DataTable reclamosAutos, DataTable bitacora, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("reclamos");

                for (int i = 0; i < reclamosAutos.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = reclamosAutos.Columns[i].ColumnName;
                }

                for (int i = 0; i < reclamosAutos.Rows.Count; i++)
                {
                    for (int j = 0; j < reclamosAutos.Columns.Count; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = reclamosAutos.Rows[i][j].ToString();
                    }
                }

                var worksheetTwo = workbook.Worksheets.Add("bitacoras");
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

                // Guardar el archivo Excel
                workbook.SaveAs(filePath);
            }
        }
    }
}
