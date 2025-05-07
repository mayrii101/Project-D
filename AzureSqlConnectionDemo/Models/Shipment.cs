using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AzureSqlConnectionDemo.Models;
namespace AzureSqlConnectionDemo.Models
{
    public class Shipment
    {
        public int Id { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public int DriverId { get; set; }

        // Remove [Required] here
        public Vehicle Vehicle { get; set; } = default!;

        // Remove [Required] here
        public Employee Driver { get; set; } = default!;

        public ShipmentStatus Status { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<ShipmentOrder> ShipmentOrders { get; set; } = new List<ShipmentOrder>();

        [NotMapped]
        public ICollection<Order> Orders => ShipmentOrders.Select(so => so.Order).ToList();
    }

    public class ShipmentOrder
    {
        [Required]
        public int ShipmentId { get; set; }

        // Remove [Required] here
        public Shipment Shipment { get; set; } = default!;

        [Required]
        public int OrderId { get; set; }

        // Remove [Required] here
        public Order Order { get; set; } = default!;
    }
}