using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class PheripheralActivity: BaseEntity
    {
        public DateTime DateCreated { get; set; }
        public double MouseActivePercentage { get; set; }
        public double KeyboardActivePercentage { get; set; }
        public Guid RecorderId { get; set; }
        public RecorderRegistration Recorder { get; set; }
    }
}
