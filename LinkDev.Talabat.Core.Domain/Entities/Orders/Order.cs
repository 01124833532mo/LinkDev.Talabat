namespace LinkDev.Talabat.Core.Domain.Entities.Orders
{
    public class Order : BaseAuditableEntity<int>
    {
        public required string BuyerEmail { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public required Address ShippingAddress { get; set; }

        public int? DeliveryMethodId { get; set; }

        public virtual DeliveryMethod? DeliveryMethod { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();

        public decimal Subtotal { get; set; }
        //[NotMapped]
        //public decimal Total => Subtotal + DeliveryMethod!.Cost;

        // derived attribute
        public decimal GetTotal => Subtotal + DeliveryMethod!.Cost;

        public string PaymentIntentId { get; set; } = "";
    }
}
