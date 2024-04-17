public record Reservation
{
    public Room ?Room { get; init; }
    public DateTime Date { get; init; }
    public DateTime Time { get; init; }
    public string ?ReserverName { get; init; }
}