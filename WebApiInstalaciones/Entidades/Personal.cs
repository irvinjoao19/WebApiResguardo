using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Personal
    {
        public int personalId { get; set; }
        public int empresaId { get; set; }
        public int tipoDocId { get; set; }
        public string nroDocumento { get; set; }
        public string apellidos { get; set; }
        public string nombre { get; set; }
        public int cargoId { get; set; }
        public string direccion { get; set; }
        public int distritoId { get; set; }
        public int estado { get; set; }
        public int usuarioId { get; set; }
    }
}
