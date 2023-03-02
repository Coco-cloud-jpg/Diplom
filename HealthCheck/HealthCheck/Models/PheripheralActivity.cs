using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.Models
{
    internal class PheripheralActivity
    {
        public Guid RecorderId { get; set; }
        public double MouseActivity { get; set; }
        public double KeyboardActivity { get; set; }
    }
}
