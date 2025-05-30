﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Enum;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.Booking;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Booking;
using GymRadar.Model.Utils;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymRadar.Service.Implement
{
    public class BookingService : BaseService<BookingService>, IBookingService
    {
        public BookingService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<BookingService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<CreateBookingResponse>> CreateBooking(CreateBookingRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.Active == true);

            if (account == null)
            {
                return new BaseResponse<CreateBookingResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản người dùng",
                    data = null
                };
            }

            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
                predicate: u => u.AccountId.Equals(accountId) && u.Active == true);

            if (user == null)
            {
                return new BaseResponse<CreateBookingResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người dùng",
                    data = null
                };
            }

            if (request.Date < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new BadHttpRequestException("Không thể đặt lịch cho ngày trong quá khứ");
            }

            var ptSlot = await _unitOfWork.GetRepository<Ptslot>().SingleOrDefaultAsync(
                predicate: pts => pts.Id.Equals(request.PtSlotId) && pts.Active == true);

            if (ptSlot == null)
            {
                return new BaseResponse<CreateBookingResponse>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy khung giờ của pt",
                    data = null
                };
            }

            var existBooking = await _unitOfWork.GetRepository<Booking>().SingleOrDefaultAsync(
                predicate: b => b.PtSlotId.Equals(request.PtSlotId) && b.Active == true && b.Date.Equals(request.Date));

            if (existBooking != null)
            {
                return new BaseResponse<CreateBookingResponse>
                {
                    status = StatusCodes.Status400BadRequest.ToString(),
                    message = "Đã có lịch đặt cho khung giờ này với PT này vào ngày này",
                    data = null
                };
            }

            var booking = _mapper.Map<Booking>(request);
            booking.UserId = user.Id;

            await _unitOfWork.GetRepository<Booking>().InsertAsync(booking);
            await _unitOfWork.CommitAsync();

            return new BaseResponse<CreateBookingResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Booking thành công",
                data = _mapper.Map<CreateBookingResponse>(booking)
            };
        }

        public async Task<BaseResponse<GetBookingResponse>> GetBookingById(Guid id)
        {
            var booking = await _unitOfWork.GetRepository<Booking>().SingleOrDefaultAsync(
                selector: b => _mapper.Map<GetBookingResponse>(b),
                predicate: b => b.Id.Equals(id) && b.Active == true,
                include: b => b.Include(b => b.User)
                               .Include(b => b.PtSlot)
                               .ThenInclude(pts => pts.Pt)
                               .Include(b => b.PtSlot.Slot));

            return new BaseResponse<GetBookingResponse>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy thông tin đặt lịch thành công",
                data = booking
            };
        }

        public async Task<BaseResponse<IPaginate<GetBookingResponse>>> GetBookingForAdmin(int page, int size)
        {
            var bookings = await _unitOfWork.GetRepository<Booking>().GetPagingListAsync(
                selector: b => _mapper.Map<GetBookingResponse>(b),
                predicate: b => b.Active == true,
                include: b => b.Include(b => b.User)
                               .Include(b => b.PtSlot)
                               .ThenInclude(pts => pts.Pt)
                               .Include(b => b.PtSlot.Slot),
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetBookingResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách đặt lịch cho PT thành công",
                data = bookings
            };
        }

        public async Task<BaseResponse<IPaginate<GetBookingResponse>>> GetBookingForPT(int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.Active == true);

            if (account == null)
            {
                return new BaseResponse<IPaginate<GetBookingResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản người dùng",
                    data = null
                };
            }

            var pt = await _unitOfWork.GetRepository<Pt>().SingleOrDefaultAsync(
                predicate: p => p.AccountId.Equals(accountId) && p.Active == true);

            if (pt == null)
            {
                return new BaseResponse<IPaginate<GetBookingResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin của PT",
                    data = null
                };
            }

            var ptSlots = await _unitOfWork.GetRepository<Ptslot>().GetListAsync(
                predicate: pts => pts.Ptid.Equals(pt.Id) && pts.Active == true);

            var ptSlotIds = ptSlots.Select(pts => pts.Id).ToList();

            var bookings = await _unitOfWork.GetRepository<Booking>().GetPagingListAsync(
                selector: b => _mapper.Map<GetBookingResponse>(b),
                predicate: b => ptSlotIds.Contains(b.PtSlotId.Value) && b.Active == true,
                include: b => b.Include(b => b.User)
                               .Include(b => b.PtSlot)
                               .ThenInclude(pts => pts.Pt)
                               .Include(b => b.PtSlot.Slot),
                page: page,
                size: size);


            return new BaseResponse<IPaginate<GetBookingResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách đặt lịch cho PT thành công",
                data = bookings
            };
        }

        public async Task<BaseResponse<IPaginate<GetBookingResponse>>> GetBookingForUser(int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.Active == true);

            if (account == null)
            {
                return new BaseResponse<IPaginate<GetBookingResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản người dùng",
                    data = null
                };
            }

            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
                predicate: u => u.AccountId.Equals(accountId) && u.Active == true);

            if (user == null)
            {
                return new BaseResponse<IPaginate<GetBookingResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy người dùng",
                    data = null
                };
            }

            var bookings = await _unitOfWork.GetRepository<Booking>().GetPagingListAsync(
                selector: b => _mapper.Map<GetBookingResponse>(b),
                predicate: b => b.UserId.Equals(user.Id) && b.Active == true,
                include: b => b.Include(b => b.User)
                               .Include(b => b.PtSlot)
                               .ThenInclude(pts => pts.Pt)
                               .Include(b => b.PtSlot.Slot),
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetBookingResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách đặt lịch cho người dùng thành công",
                data = bookings
            };
        }

        public async Task<BaseResponse<bool>> UpdateBooking(Guid id, StatusBookingEnum status)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.Active == true);

            if (account == null)
            {
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản người dùng",
                    data = false
                };
            }

            var pt = await _unitOfWork.GetRepository<Pt>().SingleOrDefaultAsync(
                predicate: p => p.AccountId.Equals(accountId) && p.Active == true);

            if (pt == null)
            {
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin của PT",
                    data = false
                };
            }

            var booking = await _unitOfWork.GetRepository<Booking>().SingleOrDefaultAsync(
                predicate: b => b.Id.Equals(id) && b.Active == true && b.PtSlot.Ptid.Equals(pt.Id),
                include: b => b.Include(b => b.PtSlot).ThenInclude(pts => pts.Pt));
            if (booking == null)
            {
                return new BaseResponse<bool>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy thông tin đặt lịch hoặc không phải của PT này",
                    data = false
                };
            }

            booking.Status = status.GetDescriptionFromEnum();
            booking.UpdateAt = TimeUtil.GetCurrentSEATime();

            _unitOfWork.GetRepository<Booking>().UpdateAsync(booking);
            await _unitOfWork.CommitAsync();

            return new BaseResponse<bool>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Cập nhật trạng thái đặt lịch thành công",
                data = true
            };
        }
    }
}
