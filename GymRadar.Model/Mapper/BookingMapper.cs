using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Enum;
using GymRadar.Model.Payload.Request.Booking;
using GymRadar.Model.Payload.Response.Booking;
using GymRadar.Model.Utils;

namespace GymRadar.Model.Mapper
{
    public class BookingMapper : Profile
    {
        public BookingMapper()
        {
            CreateMap<CreateBookingRequest, Booking>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeUtil.GetCurrentSEATime()));

            CreateMap<Booking, CreateBookingResponse>();

            CreateMap<Booking, GetBookingResponse>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.PT, opt => opt.MapFrom(src => src.PtSlot.Pt))
                .ForMember(dest => dest.Slot, opt => opt.MapFrom(src => src.PtSlot.Slot));
        }
    }
}
