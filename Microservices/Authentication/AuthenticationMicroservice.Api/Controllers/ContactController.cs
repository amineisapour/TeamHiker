using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AuthenticationMicroservice.Domain.Models;
using AuthenticationMicroservice.Domain.ViewModels;
using GiliX.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using AuthenticationMicroservice.SimplePersistence;

namespace AuthenticationMicroservice.Api.Controllers
{
    public class ContactController : Infrastructure.ApiControllerBase
    {
        #region Constructor
        public ContactController(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        #endregion

        #region Methods
        [AllowAnonymous]
        [HttpPost("send-data")]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<Result> SendData([FromBody] ContactViewModel model)
        {
            var result = new FluentResults.Result();
            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState.Values.SelectMany(modelState => modelState.Errors))
                    {
                        result.WithError(error.ErrorMessage);
                    }
                    return result.ConvertToDtxResult();
                }
                var newContact = new Contact
                {
                    Email = model.Email,
                    Fullname = model.Fullname,
                    Message = model.Message,
                    Subject = model.Subject,
                    IsActive = true
                };
                await DbContext.Contacts.AddAsync(newContact);
                await DbContext.SaveChangesAsync();

                result.WithSuccess(successMessage: string.Format(Resources.Messages.Successes.SuccessInsert, "Message"));
            }
            catch (Exception ex)
            {
                result.WithError(errorMessage: ex.Message);
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    result.WithError(ex.InnerException.Message);
                }
            }
            return result.ConvertToDtxResult();
        }
        #endregion
    }
}
