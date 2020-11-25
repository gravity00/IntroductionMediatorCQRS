using System;

namespace IntroductionMediatorCQRS.Handlers.Audits
{
    public class AuditSearchItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public long ExecutionTimeInMs { get; set; }
    }
}