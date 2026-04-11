using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsCatalogAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        [Required]
        [StringLength(50)]
        public string Sku { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
