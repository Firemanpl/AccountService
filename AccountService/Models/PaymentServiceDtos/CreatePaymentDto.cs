using System.ComponentModel.DataAnnotations;

namespace AccountService.Models.PaymentServiceDtos
{
    public class CreatePaymentDto
    {
        public int VehicleId { get; set; }
        public double Kilometers { get; set; }
        public double KWh { get; set; }
        public string Currency { get; set; }
        public decimal Payment { get; set; }
    }
}
