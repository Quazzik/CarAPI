namespace CarAPI.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Login { get; set; }
}

public class UserDataDto
{
    public string Login { get; set; }
    public string Password { get; set; }
}

public class AuthResponseDto
{
    public string Token { get; set; }
    public UserDto User { get; set; }
}