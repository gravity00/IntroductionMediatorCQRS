using System;

namespace IntroductionMediatorCQRS.Handlers.Audits
{
    public class GetAuditByIdQuery : Query<Audit>
    {
        public Guid AuditId { get; set; }
    }
}
