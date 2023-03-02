using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.Services
{
    public interface IPheripheralController
    {
        void Stop();
        void Start();
        double GetWorkPercentage();
    }
}
