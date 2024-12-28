public class ReservationViewModel
{
    public string Sector { get; set; }
    public int SpotNumber { get; set; }
    public DateTime ReservationDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string UserId { get; set; }
    public string UserEmail { get; set; }
}
