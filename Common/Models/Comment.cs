using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Comment: BaseEntity
    {
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }
        public Guid ScreenshotId { get; set; }
        public Guid CreatorId { get; set; }
        public User User { get; set; }
        public Screenshot Screenshot { get; set; }
    }
}
