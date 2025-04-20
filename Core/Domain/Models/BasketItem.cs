namespace Domain.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}