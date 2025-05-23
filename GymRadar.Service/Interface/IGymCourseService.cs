using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymRadar.Model.Paginate;
using GymRadar.Model.Payload.Request.GymCourse;
using GymRadar.Model.Payload.Response;
using GymRadar.Model.Payload.Response.GymCourse;
using GymRadar.Model.Payload.Response.PT;

namespace GymRadar.Service.Interface
{
    public interface IGymCourseService
    {
        Task<BaseResponse<CreateGymCourseResponse>> CreateNewGymCourse(CreateGymCourseRequest request);
        Task<BaseResponse<IPaginate<GetGymCourseResponse>>> GetAllGymCourse(int page, int size);
        Task<BaseResponse<IPaginate<GetGymCourseResponse>>> GetAllGymCourseForUser(Guid id, int page, int size);
        Task<BaseResponse<IPaginate<GetPTResponse>>> GetAllPTForGymCourse(Guid id, int page, int size);
    }
}
