using System.ComponentModel.DataAnnotations;

namespace Exchanger.API.Entities
{
    public class Listing
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        [Required]
        public User User { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public ICollection<ListingImages> Images { get; set; } = new List<ListingImages>();
        public ICollection<ListingCategory> Categories { get; set; } = new List<ListingCategory>();
    }
}
