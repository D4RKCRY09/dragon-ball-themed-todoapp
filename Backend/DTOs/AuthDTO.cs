namespace Backend.DTOs
{
    // DTOs/RegisterDto.cs
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // DTOs/LoginDto.cs
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
