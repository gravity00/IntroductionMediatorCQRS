using System.Collections.Generic;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class SearchProductsQuery : Query<IEnumerable<Product>>
    {
        public string FilterQ { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}
