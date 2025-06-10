using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Payload.Request.Premium;
using GymRadar.Model.Payload.Response.Premium;
using GymRadar.Model.Utils;

namespace GymRadar.Model.Mapper
{
    public class PremiumMapper : Profile
    {
        public PremiumMapper()
        {
            CreateMap<CreateNewPremiumRequest, Premium>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Premium, CreateNewPremiumResponse>();

            CreateMap<Premium, GetPremiumResponse>();
        }
    }
}
