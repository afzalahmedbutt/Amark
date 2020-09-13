using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingPortal.Core.Domain.Identity;
using TradingPortal.Core.ViewModels;

namespace TradingPortal.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, UserViewModel>()
                   .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserViewModel, Customer>()
                .ForMember(d => d.Roles, map => map.Ignore());

            CreateMap<Customer, UserEditViewModel>()
                .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserEditViewModel, Customer>()
                .ForMember(d => d.Roles, map => map.Ignore());

            //CreateMap<Customer, UserPatchViewModel>()
            //    .ReverseMap();

            CreateMap<Role, RoleViewModel>()
                //.ForMember(d => d.Permissions, map => map.MapFrom(s => s.Claims))
                .ForMember(d => d.UsersCount, map => map.ResolveUsing(s => s.Users?.Count ?? 0))
                .ReverseMap();
            CreateMap<RoleViewModel, Role>();

            //CreateMap<IdentityRoleClaim<string>, ClaimViewModel>()
            //    .ForMember(d => d.Type, map => map.MapFrom(s => s.ClaimType))
            //    .ForMember(d => d.Value, map => map.MapFrom(s => s.ClaimValue))
            //    .ReverseMap();

            //CreateMap<ApplicationPermission, PermissionViewModel>()
            //    .ReverseMap();

            //CreateMap<IdentityRoleClaim<string>, PermissionViewModel>()
            //    .ConvertUsing(s => Mapper.Map<PermissionViewModel>(ApplicationPermissions.GetPermissionByValue(s.ClaimValue)));

            //CreateMap<Customer, CustomerViewModel>()
            //    .ReverseMap();

            //CreateMap<Product, ProductViewModel>()
            //    .ReverseMap();

            //CreateMap<Order, OrderViewModel>()
            //    .ReverseMap();
        }
    }
}
