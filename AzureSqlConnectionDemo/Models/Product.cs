using System.ComponentModel.DataAnnotations;

namespace AzureSqlConnectionDemo.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(50)]
        public string SKU { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int WeightKg { get; set; }

        [StringLength(100)]
        public string Material { get; set; } = string.Empty;

        public int BatchNumber { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        public DateTime? ExpirationDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}