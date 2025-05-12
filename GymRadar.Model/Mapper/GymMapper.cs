using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Enum;
using GymRadar.Model.Payload.Request.Gym;
using GymRadar.Model.Payload.Response.Gym;
using GymRadar.Model.Utils;

namespace GymRadar.Model.Mapper
{
    public class GymMapper : Profile
    {
        public GymMapper()
        {
            CreateMap<CreateNewGymRequest, Gym>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.HotResearch, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Gym, CreateNewGymResponse>();
        }
    }
}
