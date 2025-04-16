using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications;
using Shared;

namespace Services
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper mapper) : IProductService
    {

        public async Task<PaginationResponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParameters specParameters)
        {
            var spec = new ProductWithBrandsAndTypesSpecifications(specParameters);
            // Get All Products Through ProductRepository

            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);

            var specCount = new ProductWithCountSpecifications(specParameters);

            var count = await _unitOfWork.GetRepository<Product, int>().CountAsync(specCount);

            //var count = products.Count();

            // Mapping IEnumerable<Product> to IEnumerable<ProductResultDto> : Automapper

            var result = mapper.Map<IEnumerable<ProductResultDto>>(products);
            return new PaginationResponse<ProductResultDto>(specParameters.PageIndex, specParameters.PageSize, count, result);

        }
        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandsAndTypesSpecifications(id);
            var product = await _unitOfWork.GetRepository<Product, int>().GetAsync(id);
            if (product is null)
            {
                return null;
            }
            var result = mapper.Map<ProductResultDto>(product);
            return result;
        }
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return result;
        }
        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<TypeResultDto>>(types);
            return result;
        }

    }
}
