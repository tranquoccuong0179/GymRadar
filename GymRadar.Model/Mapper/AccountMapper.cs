using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Enum;
using GymRadar.Model.Payload.Request.User;
using GymRadar.Model.Payload.Response.User;
using GymRadar.Model.Utils;

namespace GymRadar.Model.Mapper
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<RegisterRequest, Account>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordUtil.HashPassword(src.Password)))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => RoleEnum.USER.GetDescriptionFromEnum()))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Account, RegisterResponse>();

            CreateMap<RegisterAdminRequest, Account>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordUtil.HashPassword(src.Password)))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => RoleEnum.ADMIN.GetDescriptionFromEnum()))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<RegisterAccountGymRequest, Account>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordUtil.HashPassword(src.Password)))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => RoleEnum.GYM.GetDescriptionFromEnum()))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<RegisterAccountPTRequest, Account>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordUtil.HashPassword(src.Password)))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => RoleEnum.PT.GetDescriptionFromEnum()))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));
        }
    }
}
