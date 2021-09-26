using System;
using System.Diagnostics;

namespace AccountService.Entities
{
    public class UserPayments
    {
        public int Id { get; set; }
        public DateTimeOffset Time { get; set; }
        public int VehicleId { get; set; }
        public double Kilometers { get; set; }
        public double KWh { get; set; }
        public string Currency { get; set; }
        public decimal Payment { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}