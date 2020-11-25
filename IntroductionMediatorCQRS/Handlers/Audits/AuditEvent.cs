using System;

namespace IntroductionMediatorCQRS.Handlers.Audits
{
    public class AuditEvent
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public object Payload { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}