using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Core
{
    public abstract record IntegrationEvent
    {
        public DateTime OccuredOn { get; }
        public Guid EventId { get; set; }
        public IntegrationEvent()
        {
            EventId = Guid.NewGuid();
            OccuredOn = DateTime.UtcNow;
        }
    }
}
