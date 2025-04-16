using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Shared;

namespace Services.Specifications
{
    public class ProductWithCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithCountSpecifications(ProductSpecificationsParameters specParameters)
        : base(P =>
            (string.IsNullOrEmpty(specParameters.Search) || P.Name.ToLower().Contains(specParameters.Search.ToLower()))
            &&
            (!specParameters.BrandId.HasValue || P.BrandId == specParameters.BrandId)
            &&
            (!specParameters.TypeId.HasValue || P.TypeId == specParameters.TypeId))
        {

        }

    }
}
