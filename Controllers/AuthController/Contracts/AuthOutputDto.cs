namespace SisandBackend.Controllers.AuthController.Contracts;

public class AuthOutputDto
{
    public string AccessToken { get; set; } 
    public string RefreshToken { get; set; } 
    public int ExpiresIn { get; set; } 
}