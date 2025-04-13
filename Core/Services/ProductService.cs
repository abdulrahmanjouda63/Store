using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Shared;

namespace Services
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper mapper) : IProductService
    {

        public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync()
        {
            // Get All Products Through ProductRepository

            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync();

            // Mapping IEnumerable<Product> to IEnumerable<ProductResultDto> : Automapper

            var result = mapper.Map<IEnumerable<ProductResultDto>>(products);
            return result;

        }
        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
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
