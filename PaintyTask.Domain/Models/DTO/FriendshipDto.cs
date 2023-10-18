namespace PaintyTask.Domain.Models.DTO;

public class FriendshipDto
{
    public int UserSenderId { get; set; } // Идентификатор пользователя, отправившего запрос на дружбу
    public int UserReceiverId { get; set; } // Идентификатор пользователя, получившего запрос на дружбу
}