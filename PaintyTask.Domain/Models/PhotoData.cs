namespace PaintyTask.Domain.Models;

public class PhotoData
{
    public int Id { get; set; }
    public string Url { get; set; }
    public int UserId { get; set; }
    public UserData User { get; set; }
}