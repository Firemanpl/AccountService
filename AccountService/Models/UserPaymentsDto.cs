namespace AccountService.Models
{
    public class UserPaymentsDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public double Kilometers { get; set; }
        public double KWh { get; set; }
        public string Currency { get; set; }
        public decimal Payment { get; set; }
    }
}