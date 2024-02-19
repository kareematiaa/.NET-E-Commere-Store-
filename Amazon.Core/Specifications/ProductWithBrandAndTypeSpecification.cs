using Amazon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification :BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecification(ProductSpecParams SpecParams)
            :base(P => 
                    (string.IsNullOrEmpty(SpecParams.Search) || P.Name.ToLower().Contains(SpecParams.Search)) &&
                    (!SpecParams.brandId.HasValue || P.productBrandId == SpecParams.brandId.Value) &&
                    (!SpecParams.typeId.HasValue || P.productTypeId == SpecParams.typeId.Value) 
            )
        {
            Includes.Add(P => P.productBrand);
            Includes.Add(P => P.productType); 


             if(!string.IsNullOrEmpty(SpecParams.sort))
            {
                switch (SpecParams.sort)
                {
                    case "priceAsc":
                        AddOrederBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrederByDesc(P => P.Price);
                        break;
                    default:
                        AddOrederBy(P => P.Name);
                        break;    
                }
            }


            ApplyPagination(SpecParams.PageSize * (SpecParams.PageIndex - 1),SpecParams.PageSize);
        }
        public ProductWithBrandAndTypeSpecification(int id):base(P => P.Id == id)
        {
            Includes.Add(P => P.productBrand);
            Includes.Add(P => P.productType); 
        }
    }
}
