using System;
using System.Collections.Generic;

namespace IntroductionMediatorCQRS.Handlers.Audits
{
    public class Audit
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public object Payload { get; set; }

        public object Result { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public long ExecutionTimeInMs { get; set; }

        public IEnumerable<AuditEvent> Events { get; set; }
    }
}