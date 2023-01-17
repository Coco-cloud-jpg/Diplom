using Common.Models;
using System;
using System.Collections.Generic;

namespace ScreenMonitorService.Models
{
    public partial class RecorderRegistration: BaseEntity
    {
        public string MacAddress { get; set; } = null!;
        public Guid? CustomerId { get; set; }
        public Guid ApiKey { get; set; }
        public DateTime TimeCreated { get; set; }

        public virtual Customer? Customer { get; set; }
    }
}
