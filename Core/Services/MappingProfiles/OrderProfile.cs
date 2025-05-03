using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models.OrderModels;
using Shared.OrderModels;

namespace Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(D => D.ProductId, O => O.MapFrom(S => S.ProductInOrderItem.ProductId))
                .ForMember(D => D.ProductName, O => O.MapFrom(S => S.ProductInOrderItem.ProductName))
                .ForMember(D => D.PictureUrl, O => O.MapFrom(S => S.ProductInOrderItem.PictureUrl))
                ;

            CreateMap<Order, OrderResultDto>()
                .ForMember(D => D.PaymentStatus, O => O.MapFrom(S => S.PaymentStatus.ToString()))
                .ForMember(D => D.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(D => D.Total, O => O.MapFrom(S => S.SubTotal + S.DeliveryMethod.Cost));

            CreateMap<DeliveryMethod, DeliveryMethodDto>();
        }
    }
}
