#nullable disable


namespace DAL.Models;

public partial class Log
{
    public int Id { get; set; }

    public string Message { get; set; }

    public string Level { get; set; }

    public DateTime Timestamp { get; set; }
}