namespace BookAdvisor.Application.DTOs.Auth
{
    public record AuthResponse(
         string UserId,
         string Token,
         bool Success,
         string ErrorMessage
     );
}
