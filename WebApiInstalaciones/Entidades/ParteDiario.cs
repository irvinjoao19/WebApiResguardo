using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ParteDiario
    {
        public int parteDiarioId { get; set; }
        public int empresaId { get; set; }
        public int servicioId { get; set; }
        public string fechaAsignacion { get; set; }
        public string fechaRegistro { get; set; }
        public string horaInicio { get; set; }
        public string horaTermino { get; set; }
        public string totalHoras { get; set; }
        public decimal cantidadHoras { get; set; }
        public decimal precio { get; set; }
        public decimal totalSoles { get; set; }
        public int usuarioEfectivoPolicialId { get; set; }
        public int personalCoordinarId { get; set; }
        public int personalJefeCuadrillaId { get; set; }
        public string lugarTrabajoPD { get; set; }
        public string nroObraTD { get; set; }
        public string observacionesPD { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string firmaEfectivoPolicial { get; set; }
        public string firmaJefeCuadrilla { get; set; }
        public int estadoId { get; set; }
        public int identity { get; set; }
        public int usuarioId { get; set; }
        public string nombreJefeCuadrilla { get; set; }
        public string nombreCoordinador { get; set; }
        public string nombreServicio { get; set; }
        public string nombreEstado { get; set; }
        public string incidencia { get; set; }
        public List<Personal> personals { get; set; }
        public List<ParteDiarioPhoto> photos { get; set; }
    }
}