using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Payload.Response.Transaction;

namespace GymRadar.Model.Mapper
{
    public class TransactionMapper : Profile
    {
        public TransactionMapper()
        {
            CreateMap<Transaction, GetTransactionResponse>()
                .ForMember(dest => dest.Gym, opt => opt.MapFrom(src => src.GymCourse.Gym));
        }
    }
}
