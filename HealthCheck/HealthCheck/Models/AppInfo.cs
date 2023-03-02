using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.Models
{
    internal class AppInfoBase
    {
        public string IconBase64 { get; set; }
        public uint Seconds { get; set; }
    }
    internal class AppFullInfo: AppInfoBase
    {
        public string Name { get; set; }
    }

    internal class AppInfoSTransferDTO
    {
        public IEnumerable<AppFullInfo> AppsInfo { get; set; }
        public Guid RecorderId { get; set; }
    }
}
