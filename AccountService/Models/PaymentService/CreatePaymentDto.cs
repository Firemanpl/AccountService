using System.ComponentModel.DataAnnotations;

namespace AccountService.Models.PaymentService
{
    public class CreatePaymentDto
    {
        [Required]
        public int VehicleId { get; set; }
        [Required]
        public double Kilometers { get; set; }
        [Required]
        public double KWh { get; set; }
        [Required]
        [MaxLength(3)]
        public string Currency { get; set; }
        [Required]
        public decimal Payment { get; set; }
    }
}
