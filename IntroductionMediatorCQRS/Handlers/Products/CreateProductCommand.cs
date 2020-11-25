namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class CreateProductCommand : Command<CreateProductResult>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
