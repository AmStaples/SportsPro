using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsPro.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Please enter a product code.")]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "Please enter a name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a yearly price.")]
        [Range(0.01, 1000000, ErrorMessage = "Please enter a yearly price between $0.01 and $1,000,000.")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal? YearlyPrice { get; set; } = 0;

        [Required(ErrorMessage = "Please enter a release date.")]
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
    }
}
