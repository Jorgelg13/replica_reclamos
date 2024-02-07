
using System.Collections.Generic;

namespace ReplicaDataReclamos
{
    internal class Consultas
    {
        public static string CADENA_CONEXION()
        {
            return "Persist Security Info=False;User ID=sa;Password=admin123;Initial Catalog=reclamos;Server=alienware\\SQLEXPRESS";
        }
        public static string RECLAMOS_AUTOS()
        {
            return "select " +
                    "r.id," +
                    "r.boleta," +
                    "r.titular," +
                    "r.ubicacion," +
                    "r.hora," +
                    "r.fecha," +
                    "r.reportante," +
                    "r.piloto," +
                    "r.edad," +
                    "r.telefono," +
                    "r.ajustador," +
                    "r.version," +
                    "r.metodo," +
                    "u.nombre as usuario," +
                    "r.fecha_commit as fecha_registro,"+
                    "r.hora_commit as hora_registro,"+
                    "r.tipo_servicio," +
                    "a.poliza," +
                    "a.placa," +
                    "a.modelo," +
                    "a.marca," +
                    "a.chasis," +
                    "a.motor," +
                    "a.propietario," +
                    "a.ejecutivo," +
                    "a.aseguradora," +
                    "a.asegurado " +
                    "from reclamo_auto as r " +
                    "inner join auto_reclamo as a on r.id_auto_reclamo = a.id " +
                    "inner join usuario as u on u.id = r.id_usuario " +
                    "where r.id_estado = 2 and replica_ibis = 0";
        }

        public static string RECLAMOS_VARIOS()
        {
            return "Select " +
                "r.id," +
                "r.boleta," +
                "r.titular," +
                "r.ubicacion," +
                "r.tipo_servicio,"+
                "r.hora," +
                "r.fecha," +
                "r.reportante," +
                "r.telefono," +
                "r.ajustador," +
                "r.version," +
                "r.metodo," +
                "u.nombre as usuario," +
                "r.fecha_commit as fecha_registro," +
                "r.hora_commit as hora_registro," +
                "rv.poliza," +
                "rv.asegurado," +
                "rv.cliente," +
                "rv.ramo," +
                "rv.ejecutivo," +
                "rv.aseguradora," +
                "rv.contratante," +
                "rv.moneda " +
                "from reclamos_varios as r " +
                "inner join reg_reclamo_varios as rv on r.id_reg_reclamos_varios = rv.id " +
                "inner join usuario as u on u.id = r.id_usuario " +
                "where r.id_estado = 2 and replica_ibis = 0";
        }

        public static string RECLAMOS_MEDICOS()
        {
            return "select top(100) " +
                "r.id,"+
                "r.asegurado," +
                "r.titular," +
                "r.telefono," +
                "r.correo," +
                "r.empresa," +
                "r.metodo," +
                "u.nombre as usuario," +
                "r.fecha_completa_commit as fecha_registro," +
                "rm.poliza,rm.ramo," +
                "rm.tipo," +
                "rm.clase," +
                "rm.ejecutivo," +
                "rm.aseguradora," +
                "rm.moneda," +
                "rm.certificado " +
                "from reclamos_medicos as r " +
                "inner join reg_reclamos_medicos as rm on rm.id = r.id_reg_reclamos_medicos " +
                "inner join usuario as u on u.id = r.id_usuario " +
                "where r.id_estado = 2 and replica_ibis = 0";
        }

        public static string BITACORA_AUTOS(List<int> ids)
        {
            return "SELECT " +
               "id,"+
               "descripcion, " +
               "fecha_commit as fecha_registro, " +
               "hora_commit as hora_registro," +
               "id_reclamo, " +
               "usuario FROM bitacora_reclamo_auto " +
               "WHERE id_reclamo in (" + string.Join(",", ids) + ")";
        }

        public static string BITACORA_RECLAMOS_VARIOS(List<int> ids)
        {
            return "SELECT " +
                "id," +
                "descripcion, " +
                "fecha_commit as fecha_registro, " +
                "hora_commit as hora_registro," +
                "id_reclamos_varios, " +
                "usuario FROM bitacora_reclamos_varios " +
                "WHERE id_reclamos_varios in (" + string.Join(",", ids) + ")";
        }

        public static string RECIBOS_MEDICOS(List<int> ids)
        {
            return "SELECT " +
                "id, " +
                "descripcion as tipo_documento, " +
                "comentarios, " +
                "cantidad, " +
                "id_reclamo_medico " +
                "FROM recibos_medicos " +
                "WHERE id_reclamo_medico in (" + string.Join(",", ids) + ")";
        }
    }
}
