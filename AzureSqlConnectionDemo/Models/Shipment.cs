using System.ComponentModel.DataAnnotations.Schema;

namespace AzureSqlConnectionDemo.Models
{
    public class Shipment
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }

        public int DriverId { get; set; }

        public Vehicle Vehicle { get; set; } = default!;

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
        public int ShipmentId { get; set; }

        public Shipment Shipment { get; set; } = default!;

        public int OrderId { get; set; }

        public Order Order { get; set; } = default!;
    }
}