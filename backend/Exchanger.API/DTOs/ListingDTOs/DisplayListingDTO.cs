using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Entities;

namespace Exchanger.API.DTOs.ListingDTOs
{
    public class DisplayListingDTO
    {
        public Guid ListingId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; } = new List<string>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActived { get; set; }
        public DisplayUserInfoDTO PublisherInfo { get; set; } = new DisplayUserInfoDTO();
    }
}
