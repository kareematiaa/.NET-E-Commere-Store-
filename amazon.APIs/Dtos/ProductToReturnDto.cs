using Amazon.Core.Entities;

namespace amazon.APIs.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }


        public int productBrandId { get; set; } 
        public string productBrand { get; set; } 


        
        public int productTypeId { get; set; } 
        public string productType { get; set; } 
    }
}
