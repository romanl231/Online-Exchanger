namespace Exchanger.API.DTOs.AuthDTOs
{
    public class LoginRequestDTO
    {
        public AuthDTO AuthDTO {  get; set; } = new AuthDTO();
        public SessionInfo SessionInfo { get; set; } = new SessionInfo();
    }
}
