namespace Exchanger.API.DTOs.AuthDTOs
{
    public class LoginRequestDTO
    {
        public AuthDTO AuthDTO {  get; set; }
        public SessionInfo SessionInfo { get; set; }
    }
}
