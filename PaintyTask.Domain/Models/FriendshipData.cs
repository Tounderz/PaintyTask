namespace PaintyTask.Domain.Models;

public class FriendshipData
{
    public int UserSenderId { get; set; } // Идентификатор пользователя, отправившего запрос на дружбу
    public int UserReceiverId { get; set; } // Идентификатор пользователя, получившего запрос на дружбу
    public bool IsAccepted { get; set; } = false; // Флаг, указывающий, принята ли дружба
    public UserData UserSender { get; set; } // Ссылка на пользователя, отправившего запрос на дружбу
    public UserData UserReceiver { get; set; } // Ссылка на пользователя, получившего запрос на дружбу
}