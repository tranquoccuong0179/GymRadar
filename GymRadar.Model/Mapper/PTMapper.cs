using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Payload.Request.PT;
using GymRadar.Model.Payload.Response.PT;
using GymRadar.Model.Payload.Response.Transaction;
using GymRadar.Model.Utils;

namespace GymRadar.Model.Mapper
{
    public class PTMapper : Profile
    {
        public PTMapper()
        {
            CreateMap<CreateNewPTRequest, Pt>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.GetDescriptionFromEnum()))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Pt, CreateNewPTResponse>();

            CreateMap<Pt, GetPTResponse>();

            CreateMap<Pt, PTResponse>();
        }
    }
}
