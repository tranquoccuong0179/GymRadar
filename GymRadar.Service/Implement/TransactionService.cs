using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.Transaction;
using GymRadar.Model.Utils;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymRadar.Service.Implement
{
    public class TransactionService : BaseService<TransactionService>, ITransactionService
    {
        public TransactionService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<TransactionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<BaseResponse<IPaginate<GetTransactionResponse>>> GetAllTransactionForAdmin(int page, int size)
        {
            var transactions = await _unitOfWork.GetRepository<Transaction>().GetPagingListAsync(
                selector: t => _mapper.Map<GetTransactionResponse>(t),
                include: t => t.Include(t => t.GymCourse)
                               .ThenInclude(gc => gc.Gym)
                               .Include(t => t.Pt)
                               .Include(t => t.User)
                               .ThenInclude(u => u.Account),
                page: page,
                size: size);

            return new BaseResponse<IPaginate<GetTransactionResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách giao dịch thành công",
                data = transactions
            };
        }

        public async Task<BaseResponse<IPaginate<GetTransactionResponse>>> GetAllTransactionForGym(int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.Active == true);

            if (account == null)
            {
                return new BaseResponse<IPaginate<GetTransactionResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản",
                    data = null
                };
            }

            var gym = await _unitOfWork.GetRepository<Gym>().SingleOrDefaultAsync(
                predicate: g => g.AccountId.Equals(accountId) && g.Active == true);

            if (gym == null)
            {
                return new BaseResponse<IPaginate<GetTransactionResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy phòng gym",
                    data = null
                };
            }

            var transactions = await _unitOfWork.GetRepository<Transaction>().GetPagingListAsync(
                selector: t => _mapper.Map<GetTransactionResponse>(t),
                predicate: t => t.GymCourse.GymId.Equals(gym.Id),
                include: t => t.Include(t => t.GymCourse)
                               .ThenInclude(gc => gc.Gym)
                               .Include(t => t.Pt)
                               .Include(t => t.User)
                               .ThenInclude(u => u.Account),
                page: page,
                size: size);
            return new BaseResponse<IPaginate<GetTransactionResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách giao dịch thành công",
                data = transactions
            };
        }

        public async Task<BaseResponse<IPaginate<GetTransactionResponse>>> GetAllTransactionForUser(int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);

            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.Active == true);

            if (account == null)
            {
                return new BaseResponse<IPaginate<GetTransactionResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy tài khoản",
                    data = null
                };
            }

            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
                predicate: u => u.AccountId.Equals(accountId) && u.Active == true);

            if (user == null)
            {
                return new BaseResponse<IPaginate<GetTransactionResponse>>
                {
                    status = StatusCodes.Status404NotFound.ToString(),
                    message = "Không tìm thấy hồ sơ",
                    data = null
                };
            }

            var transactions = await _unitOfWork.GetRepository<Transaction>().GetPagingListAsync(
                selector: t => _mapper.Map<GetTransactionResponse>(t),
                predicate: t => t.UserId.Equals(user.Id),
                include: t => t.Include(t => t.GymCourse)
                               .ThenInclude(gc => gc.Gym)
                               .Include(t => t.Pt)
                               .Include(t => t.User)
                               .ThenInclude(u => u.Account),
                page: page,
                size: size);
            return new BaseResponse<IPaginate<GetTransactionResponse>>
            {
                status = StatusCodes.Status200OK.ToString(),
                message = "Lấy danh sách giao dịch thành công",
                data = transactions
            };
        }
    }
}
