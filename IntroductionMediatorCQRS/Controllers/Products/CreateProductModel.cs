namespace IntroductionMediatorCQRS.Controllers.Products
{
    public class CreateProductModel
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}