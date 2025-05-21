using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Payload.Request.GymCoursePT;
using GymRadar.Model.Payload.Response.GymCoursePT;
using GymRadar.Model.Utils;

namespace GymRadar.Model.Mapper
{
    public class GymCoursePTMapper : Profile
    {
        public GymCoursePTMapper()
        {
            CreateMap<CreateGymCoursePTRequest, GymCoursePt>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<GymCoursePt, CreateGymCoursePTResponse>();

            CreateMap<GymCoursePt, GetGymCoursePTResponse>()
                .ForMember(dest => dest.PT, opt => opt.MapFrom(src => src.Pt))
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.GymCourse));
        }
    }
}
