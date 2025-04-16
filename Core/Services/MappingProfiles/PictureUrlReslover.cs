using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Execution;
using AutoMapper.Internal;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Shared;

namespace Services.MappingProfiles
{
    public class PictureUrlReslover(IConfiguration configuration) : IValueResolver<Product,ProductResultDto, string>
    {
        public string Resolve(Product source, ProductResultDto destination, string destMember, ResolutionContext context)
        {
            if(string.IsNullOrEmpty(source.PictureUrl))
            {
                return null;
            }
            else
            {
                return $"{configuration["BaseUrl"]}/{source.PictureUrl}";
            }
        }
    }
}
