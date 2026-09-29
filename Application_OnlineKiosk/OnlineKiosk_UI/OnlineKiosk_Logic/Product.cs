namespace OnlineKiosk_Logic
{
    public class Product
    {
        public int ProductId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string imageURL { get; private set; }
        public List<string> allergens { get; private set; }


    }
}
