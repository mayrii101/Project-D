using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzureSqlConnectionDemo.Models
{
    public class OrderLine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = default!;

        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = default!;

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public bool IsDeleted { get; private set; } = false;

        [NotMapped]
        public double LineTotal => Product.Price * Quantity;

        public void SoftDelete() => IsDeleted = true;
    }
}