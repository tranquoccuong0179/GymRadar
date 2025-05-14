using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Payload.Request.GymCourse;
using GymRadar.Model.Payload.Response.GymCourse;
using GymRadar.Model.Utils;

namespace GymRadar.Model.Mapper
{
    public class GymCourseMapper : Profile
    {
        public GymCourseMapper()
        {
            CreateMap<CreateGymCourseRequest, GymCourse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.GetDescriptionFromEnum()))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<GymCourse, CreateGymCourseResponse>();

            CreateMap<GymCourse, GetGymCourseResponse>();

        }
    }
}
