namespace Exchanger.API.DTOs.ListingDTOs
{
    public class PaginationDTO
    {
        public Guid? LastId { get; set; }
        public int Limit { get; set; } = 15;
    }
}
