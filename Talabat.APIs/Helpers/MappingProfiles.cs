﻿using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
               .ForMember(d => d.productBrand, o => o.MapFrom(s => s.Brand.Name))
               .ForMember(d => d.productType, o => o.MapFrom(s => s.Category.Name))
               .ForMember(d => d.PictureUrl, o => o.MapFrom<productPictureUrlResolver>());

        }

    }
}