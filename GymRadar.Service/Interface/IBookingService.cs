using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.Booking;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Booking;

namespace GymRadar.Service.Interface
{
    public interface IBookingService
    {
        Task<BaseResponse<CreateBookingResponse>> CreateBooking(CreateBookingRequest request);

        Task<BaseResponse<IPaginate<GetBookingResponse>>> GetBookingForUser(int page, int size);

        Task<BaseResponse<IPaginate<GetBookingResponse>>> GetBookingForPT(int page, int size);
    }
}
