namespace Basket.Core.Entities
{
    public class BasketCheckout
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal FirstName { get; set; }
        public decimal LastName { get; set; }
        public decimal EmailAddress { get; set; }
        public decimal AddressLine { get; set; }
        public decimal Country { get; set; }
        public decimal State { get; set; }
        public decimal ZipCode { get; set; }
        public decimal CardName { get; set; }
        public decimal CardNumber { get; set; }
        public decimal Expiration { get; set; }
        public decimal CVV { get; set; }
        public decimal PaymentMethod { get; set; }
    }
}
