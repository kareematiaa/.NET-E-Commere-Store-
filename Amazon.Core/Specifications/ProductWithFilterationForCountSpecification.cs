using Amazon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Specifications
{
    public class ProductWithFilterationForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecParams specParams)
            : base(P =>
                     (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
                    (!specParams.brandId.HasValue || P.productBrandId == specParams.brandId.Value) &&
                    (!specParams.typeId.HasValue || P.productTypeId == specParams.typeId.Value)
            )
        {


        }

    }
}
