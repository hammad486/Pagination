using System.ComponentModel.DataAnnotations;

namespace Pagination.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool? IsAvailable { get; set; }
        public string? Brand { get; set; }
        public string? ImageUrl { get; set; }
    }
}
