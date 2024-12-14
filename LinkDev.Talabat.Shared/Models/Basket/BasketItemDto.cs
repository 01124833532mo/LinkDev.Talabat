using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Shared.Models.Basket
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage ="Product Name is requierd")]

        public required string ProductName { get; set; }

        public string? PictureUrl { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price Must be greater than zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity Must be greater than One Item")]
        public int Quantity { get; set; }

        public string? Brand { get; set; }

        public string? Category { get; set; }
    }
}
