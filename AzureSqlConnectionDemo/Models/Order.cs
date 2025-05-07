using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzureSqlConnectionDemo.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        // Removed [Required] here. The foreign key is required (CustomerId), but the navigation property itself is optional.
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = default!;  // This is fine as it will be automatically populated if the foreign key is valid.

        [Required]
        public DateTime OrderDate { get; set; }

        [Required, StringLength(250)]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required]
        public DateTime ExpectedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public bool IsDeleted { get; private set; } = false;

        public ICollection<OrderLine> ProductLines { get; set; } = new List<OrderLine>();

        public ICollection<ShipmentOrder> ShipmentOrders { get; set; } = new List<ShipmentOrder>();

        [NotMapped]
        public int TotalWeight => ProductLines
            .Where(pl => pl.Product != null)
            .Sum(pl => (pl.Product?.WeightKg ?? 0) * pl.Quantity);

        public void SoftDelete() => IsDeleted = true;
    }
}