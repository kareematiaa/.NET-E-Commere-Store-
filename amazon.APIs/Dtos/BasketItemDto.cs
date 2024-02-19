using System.ComponentModel.DataAnnotations;

namespace amazon.APIs.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PicureUrl { get; set; }
     

        [Required]
        [Range(0.1, int.MaxValue, ErrorMessage = "Price must be greater than zero !")]
        public decimal Price { get; set; }


        [Required]
        [Range(1,int.MaxValue,ErrorMessage = "Quantity must be ine item at least !")]
        public int Quantity { get; set; }

        [Required]
        public string Brand { get; set; }


        [Required]
        public string Type { get; set; }


       
    }
}