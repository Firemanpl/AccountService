using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountService.Models
{
    public class CreateUserPaymentDto
    {
        public int VehicleId { get; set; }
        public double Kilometers { get; set; }
        public double KWh { get; set; }
        public string Currency { get; set; }
        public decimal Payment { get; set; }
        public int UserId { get; set; }
    }
}