using System.Collections.Generic;

namespace IntroductionMediatorCQRS.Handlers.Audits
{
    public class SearchAuditsQuery : Query<IEnumerable<AuditSearchItem>>
    {
        public string FilterQ { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}
