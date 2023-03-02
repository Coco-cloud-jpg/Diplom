using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.Models
{
    public class EntranceCreateDTO
    {
        public Guid RecorderId { get; set; }
        public uint Seconds { get; set; }
    }
}
