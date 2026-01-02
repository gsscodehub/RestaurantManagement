using AutoMapper;
using RestaurantManagement.Application.DTOs;
using RestaurantManagement.Domain.Entities;
using System;
using System.Data;
using System.Linq.Expressions;

namespace RestaurantManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();



            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Page, MenuItemDto>()
          .ForMember(d => d.PageId, o => o.MapFrom(s => s.Id))
          .ReverseMap()
          .ForMember(d => d.Id, o => o.MapFrom(s => s.PageId));

          CreateMap<UserRole, UserRoleDto>().ReverseMap();





        }
    }

    // ------------------ AutoMapper Ignore Extension ------------------
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Cleaner syntax for ignoring destination members
        /// Usage: CreateMap<A, B>().Ignore(x => x.Property)
        /// </summary>
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>> selector)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            map.ForMember(selector, opt => opt.Ignore());
            return map;
        }
    }
}
