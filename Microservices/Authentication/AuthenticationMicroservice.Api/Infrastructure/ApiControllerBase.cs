using AuthenticationMicroservice.SimplePersistence;
using GiliX.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace AuthenticationMicroservice.Api.Infrastructure
{
    [Microsoft.AspNetCore.Mvc.ApiController]
    [Microsoft.AspNetCore.Mvc.Route(template: "api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        //protected Persistence.IUnitOfWork UnitOfWork { get; }
        //protected Persistence.IQueryUnitOfWork QueryUnitOfWork { get; }

        //public ApiControllerBase(Persistence.IUnitOfWork unitOfWork, 
        //    Persistence.IQueryUnitOfWork queryUnitOfWork)
        //{
        //    UnitOfWork = unitOfWork;
        //    QueryUnitOfWork = queryUnitOfWork;
        //}

        private ApplicationDbContext _dbContext;
        protected ApplicationDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = new ApplicationDbContext();
                }
                return _dbContext;
            }
        }

        //protected ApplicationDbContext DbContext { get; }

        public ApiControllerBase(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected void DbContextDispose()
        {
            _dbContext.Dispose();
        }

        public string GetThis([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            return this.GetType().FullName + "." + memberName + "()";
        }

        public string GetAllErrorMessages(Exception ex)
        {
            var msg = ex.Message;
            if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
            {
                msg += $", Inner: {ex.InnerException.Message}";
            }
            return msg;
        }
    }
}
