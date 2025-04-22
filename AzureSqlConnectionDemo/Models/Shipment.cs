using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzureSqlConnectionDemo.Models
{
    public class Shipment
    {
        [Key]
        public int Id { get; set; }

        public List<Order> Orders { get; set; } = new();

        [Required]
        public Vehicle Vehicle { get; set; }

        [Required]
        public Employee Driver { get; set; }

        [Required]
        public ShipmentStatus Status { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }
    }
}