namespace LinkDev.Talabat.Core.Application.Abstraction.Models.Auth
{
    public class UserDto
    {
        public required string Id { get; set; }

        public required string DisplayName { get; set; }

        public required string Email { get; set; }

        public required string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpirationDate { get; set; }


    }
}
