using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ParteDiarioPhoto
    {
        public int photoId { get; set; }
        public int parteDiarioId { get; set; }
        public string fotoUrl { get; set; }
        public int estado { get; set; }
        public int identity { get; set; }
    }
}
