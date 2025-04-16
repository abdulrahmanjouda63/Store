using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Shared;

namespace Services.Specifications
{
    public class ProductWithBrandsAndTypesSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithBrandsAndTypesSpecifications(int id) : base(P => P.Id == id)
        {

            ApplyIncludes();
        }
        public ProductWithBrandsAndTypesSpecifications(ProductSpecificationsParameters specParameters)
            : base(P =>
            (string.IsNullOrEmpty(specParameters.Search) || P.Name.ToLower().Contains(specParameters.Search.ToLower()))
            &&
            (!specParameters.BrandId.HasValue || P.BrandId == specParameters.BrandId)
            &&
            (!specParameters.TypeId.HasValue || P.TypeId == specParameters.TypeId))
        {
            ApplyIncludes();
            ApplySorting(specParameters.Sort);
            ApplyPagination(specParameters.PageIndex, specParameters.PageSize);
        }
        private void ApplyIncludes()
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDescending(P => P.Name);
                        break;
                    case "priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }
        }
    }
}
