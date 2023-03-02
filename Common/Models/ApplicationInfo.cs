using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ApplicationInfo: BaseEntity
    {
        public ApplicationInfo()
        {
            ApplicationUsageInfo = new HashSet<ApplicationUsageInfo>();
        }
        public string Name { get; set; }
        public string IconBase64 { get; set; }
        public ICollection<ApplicationUsageInfo> ApplicationUsageInfo { get; set; }
        public override bool Equals(object obj) => (obj as ApplicationInfo).Name == this.Name;
    }
}
