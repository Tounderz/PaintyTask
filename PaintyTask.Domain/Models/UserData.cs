namespace PaintyTask.Domain.Models;

public class UserData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public ICollection<FriendshipData> SentFriendshipRequests { get; set; } // Дружбы, где пользователь отправитель
    public ICollection<FriendshipData> ReceivedFriendshipRequests { get; set; } // Дружбы, где пользователь получатель

    private readonly List<PhotoData> _photos = new ();

    public IReadOnlyCollection<PhotoData> Photos => _photos.AsReadOnly();

    public void AddPhoto(PhotoData photo)
    {
        if (photo.UserId != Id)
        {
            throw new ArgumentException("Cannot add photo not belonging to this user.");
        }    
            
        _photos.Add(photo);
    }
}