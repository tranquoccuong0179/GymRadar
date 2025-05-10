using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GymRadar.Service
{
    public abstract class BaseService<T> where T : class
    {
        protected IUnitOfWork<GymRadarContext> _unitOfWork;
        protected ILogger<T> _logger;
        protected IMapper _mapper;
        protected IHttpContextAccessor _httpContextAccessor;

        public BaseService(IUnitOfWork<GymRadarContext> unitOfWork, ILogger<T> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        protected string GetUsernameFromJwt()
        {
            var claim = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            string username = claim?.Value;
            return username;
        }

        protected string GetRoleFromJwt()
        {
            var claim = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.Role);
            string role = claim?.Value;
            return role;
        }

    }
}
