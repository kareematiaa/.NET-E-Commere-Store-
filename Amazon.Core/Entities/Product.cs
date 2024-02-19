using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }


        //[ForeignKey("ProductBrand")] //no need for that beacuse i name productType with id so no need 
        public int productBrandId { get; set; } //foreign key : not allow null 
        public ProductBrand productBrand { get; set; } //navigational proberty [one]


        //[ForeignKey("ProductType")] //no need for that beacuse i name productType with id so no need 
        public int productTypeId { get; set; } //foreign key : not allow null 
        public ProductType productType { get; set; } //navigational proberty [one]
    }
}
