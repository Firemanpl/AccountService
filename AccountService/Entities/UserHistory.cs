namespace AccountService.Entities
{
    public class UserHistory
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public double Kilometers { get; set; }
        public double KWh { get; set; }
        public bool Paid { get; set; }
        public virtual User User { get; set; }
    }
}