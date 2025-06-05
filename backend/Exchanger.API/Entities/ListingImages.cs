using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.Entities
{
    public class ListingImages
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid ListingId { get; set; }
        [Required]
        public Listing Listing { get; set; } = new Listing();

        [Required]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
