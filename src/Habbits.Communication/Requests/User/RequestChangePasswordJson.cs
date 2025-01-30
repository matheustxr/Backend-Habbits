namespace Habbits.Communication.Requests.User;

public class RequestChangePasswordJson
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;

}
