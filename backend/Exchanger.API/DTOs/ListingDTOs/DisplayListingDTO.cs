using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Entities;

namespace Exchanger.API.DTOs.ListingDTOs
{
    public class DisplayListingDTO
    {
        public Guid ListingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<Category> Categories { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActived { get; set; }
        public DisplayUserInfoDTO PublisherInfo { get; set; }
    }
}
