﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AuthenticationMicroservice.Core.Attributes;
using AuthenticationMicroservice.Domain.Enums;
using AuthenticationMicroservice.Domain.Models;
using AuthenticationMicroservice.Domain.ViewModels;
using GiliX.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using AuthenticationMicroservice.Core.Interfaces;
using AuthenticationMicroservice.SimplePersistence;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationMicroservice.Api.Controllers
{
    public class AccountController : Infrastructure.ApiControllerBase
    {
        #region Fields

        private readonly IAccountService _accountService;

        #endregion

        #region Constructor

        //public AccountController(Core.Interfaces.IAccountService accountService,
        //    Persistence.IUnitOfWork unitOfWork,
        //    Persistence.IQueryUnitOfWork queryUnitOfWork) : base(unitOfWork, queryUnitOfWork)
        //{
        //    _accountService = accountService;
        //}

        public AccountController(IAccountService accountService, ApplicationDbContext dbContext) : base(dbContext)
        {
            _accountService = accountService;
        }

        #endregion

        #region Methods

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(type: typeof(Result<AuthenticateResponse>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<Result<AuthenticateResponse>> Login([FromBody] LoginUserViewModel loginUser)
        {
            var result = new FluentResults.Result<AuthenticateResponse>();

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
                var user = await GetUserAsync(loginUser.Username);
                //User user = null;
                if (user == null)
                {
                    result.WithError(errorMessage: Resources.Messages.Errors.LoginFailed);
                    return result.ConvertToDtxResult();
                }

                var isPasswordValid = _accountService.IsPasswordValid(new PasswordViewModel
                {
                    Password = loginUser.Password,
                    PasswordSalt = user.PasswordSalt,
                    PasswordHash = user.PasswordHash

                });
                if (!isPasswordValid)
                {
                    result.WithError(errorMessage: Resources.Messages.Errors.LoginFailed);
                    return result.ConvertToDtxResult();
                }

                var jwtToken = _accountService.GenerateJwtToken(user);
                var refreshToken = _accountService.GenerateRefreshToken(IpAddress());

                //await UnitOfWork.RefreshTokens.InsertAsync(refreshToken);
                await DbContext.RefreshTokens.AddAsync(refreshToken);
                user.RefreshTokens.Add(refreshToken);

                // remove old refresh tokens from user
                //_accountService.RemoveOldRefreshTokens(user);
                RemoveOldRefreshTokens(user);

                //await UnitOfWork.Users.UpdateAsync(user);
                DbContext.Users.Update(user);

                // save changes to db
                //await UnitOfWork.SaveAsync();
                await DbContext.SaveChangesAsync();

                //var userInfo = await UnitOfWork.UserInformations.GetUserInfoByUserIdAsync(user.Id);

                var userInfo = await DbContext.UserInformations.SingleOrDefaultAsync(u => u.UserId == user.Id);
                if (userInfo == null)
                {
                    result.WithError("UserInfo Not Found!");
                    return result.ConvertToDtxResult();
                }
                var authenticateResponse = new AuthenticateResponse(user, userInfo, jwtToken, refreshToken.Token);

                result.WithSuccess(successMessage: Resources.Messages.Successes.SuccessLogin);
                result.WithValue(value: authenticateResponse);

                //SetTokenCookie(refreshToken.Token);
            }
            catch (Exception ex)
            {
                result.WithError(errorMessage: ex.Message);
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    result.WithError(ex.InnerException.Message);
                }

                //_logger.Error($"{GetThis()}: {GetAllErrorMessages(ex)}");
            }
            //finally 
            //{
            //    DbContext.Dispose();
            //}
            //System.Threading.Thread.Sleep(3000);
            return result.ConvertToDtxResult();
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [ProducesResponseType(type: typeof(Result<AuthenticateResponse>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<Result<AuthenticateResponse>> RefreshToken([FromBody] AuthenticateResponse tokenElemnt)
        {
            var result = new FluentResults.Result<AuthenticateResponse>();
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
                //Request.Headers["dd"].ToString();
                //var token = Request.Cookies["refreshToken"];
                var token = tokenElemnt.RefreshToken ?? Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(token))
                {
                    result.WithError("Token is required");
                    return result.ConvertToDtxResult();
                }

                //var response = _userService.RefreshToken(token, IpAddress());

                //var user = await QueryUnitOfWork.Users.GetUserByRefreshTokenAsync(token);
                var user = await DbContext.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
                if (user == null)
                {
                    result.WithError("Invalid token");
                    //throw new AppException("Invalid token");
                    return result.ConvertToDtxResult();
                }
                var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
                if (refreshToken.IsRevoked)
                {
                    // revoke all descendant tokens in case this token has been compromised
                    _accountService.RevokeDescendantRefreshTokens(refreshToken, user, IpAddress(), $"Attempted reuse of revoked ancestor token: {token}");

                    //await UnitOfWork.Users.UpdateAsync(user);
                    //await UnitOfWork.SaveAsync();
                    DbContext.Users.Update(user);
                    await DbContext.SaveChangesAsync();

                }

                if (!refreshToken.IsActive)
                {
                    result.WithError("Invalid token");
                    //throw new AppException("Invalid token");
                    return result.ConvertToDtxResult();
                }

                // replace old refresh token with a new one (rotate token)
                var newRefreshToken = _accountService.RotateRefreshToken(refreshToken, IpAddress());
                //await UnitOfWork.RefreshTokens.UpdateAsync(refreshToken);
                //await UnitOfWork.RefreshTokens.InsertAsync(newRefreshToken);
                DbContext.RefreshTokens.Update(refreshToken);
                await DbContext.RefreshTokens.AddAsync(newRefreshToken);

                user.RefreshTokens.Add(newRefreshToken);

                // remove old refresh tokens from user
                //_accountService.RemoveOldRefreshTokens(user);
                RemoveOldRefreshTokens(user);

                // save changes to db
                //await UnitOfWork.Users.UpdateAsync(user);
                //await UnitOfWork.SaveAsync();
                DbContext.Users.Update(user);
                await DbContext.SaveChangesAsync();

                var jwtToken = _accountService.GenerateJwtToken(user);

                //var userInfo = await UnitOfWork.UserInformations.GetUserInfoByUserIdAsync(user.Id);
                var userInfo = await DbContext.UserInformations.SingleOrDefaultAsync(u => u.UserId == user.Id);
                if (userInfo == null)
                {
                    result.WithError("UserInfo Not Found!");
                    return result.ConvertToDtxResult();
                }
                var authenticateResponse = new AuthenticateResponse(user, userInfo, jwtToken, newRefreshToken.Token);

                result.WithSuccess("Ok");
                result.WithValue(value: authenticateResponse);

                //SetTokenCookie(newRefreshToken.Token);
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

        [AllowAnonymous]
        [HttpPost("revoke-token")]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<Result> RevokeToken(RevokeTokenRequest model)
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
                // accept refresh token in request body or cookie
                var token = model.Token ?? Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(token))
                {
                    result.WithError("Token is required");
                    return result.ConvertToDtxResult();
                }

                //var user = await QueryUnitOfWork.Users.GetUserByRefreshTokenAsync(token);
                var user = await DbContext.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token)); ;
                if (user == null)
                {
                    result.WithError("Invalid token");
                    //throw new AppException("Invalid token");
                    return result.ConvertToDtxResult();
                }
                var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

                if (!refreshToken.IsActive)
                {
                    result.WithError("Invalid token");
                    //throw new AppException("Invalid token");
                    return result.ConvertToDtxResult();
                }

                // revoke token and save
                _accountService.RevokeRefreshToken(refreshToken, IpAddress(), "Revoked without replacement");

                //await UnitOfWork.Users.UpdateAsync(user);
                //await UnitOfWork.SaveAsync();
                DbContext.Users.Update(user);
                await DbContext.SaveChangesAsync();

                result.WithSuccess("Token revoked");
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

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(type: typeof(Result<AuthenticateResponse>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<Result<AuthenticateResponse>> Register([FromBody] RegisterUserViewModel registerUser)
        {
            var result = new FluentResults.Result<AuthenticateResponse>();
            //await using var transaction = await UnitOfWork.Contex.Database.BeginTransactionAsync();
            await using var transaction = await DbContext.Database.BeginTransactionAsync();
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
                var getUser = await GetUserAsync(registerUser.Username);
                if (getUser != null)
                {
                    result.WithError(string.Format(Resources.Messages.Errors.ExistEntity, "User"));
                    return result.ConvertToDtxResult();
                }

                var passwordDate = _accountService.CreatePassword(registerUser.Password);

                var newUser = new User
                {
                    Username = registerUser.Username.ToLower(),
                    PasswordHash = passwordDate.PasswordHash,
                    PasswordSalt = passwordDate.PasswordSalt
                };
                //await UnitOfWork.Users.InsertAsync(newUser);
                await DbContext.Users.AddAsync(newUser);
                var newUserInfo = new UserInformation
                {
                    Birthdate = registerUser.Birthdate,
                    FirstName = registerUser.FirstName,
                    LastName = registerUser.LastName,
                    Gender = registerUser.Gender,
                    User = newUser,
                    Email = registerUser.Email,
                };
                //await UnitOfWork.UserInformations.InsertAsync(newUserInfo);
                //await UnitOfWork.SaveAsync();
                await DbContext.UserInformations.AddAsync(newUserInfo);
                await DbContext.SaveChangesAsync();

                //var roleList = await UnitOfWork.Roles.GetAllAsync();
                var roleList = await DbContext.Roles.ToListAsync();
                var role = roleList.FirstOrDefault(p => p.Name == "User");
                if (role != null)
                {
                    var userRole = new UserRole
                    {
                        Role = role,
                        User = newUser
                    };

                    //await UnitOfWork.UserRoles.InsertAsync(userRole);
                    //await UnitOfWork.SaveAsync();
                    await DbContext.UserRoles.AddAsync(userRole);
                    await DbContext.SaveChangesAsync();

                    //Amin
                    //newUser.UserRoles = new List<UserRole> { userRole };
                    newUser.UserRoles.Add(userRole);
                }


                var jwtToken = _accountService.GenerateJwtToken(newUser);
                var refreshToken = _accountService.GenerateRefreshToken(IpAddress());

                //await UnitOfWork.RefreshTokens.InsertAsync(refreshToken);
                await DbContext.RefreshTokens.AddAsync(refreshToken);

                //Amin
                //newUser.RefreshTokens = new List<RefreshToken> { refreshToken };
                newUser.RefreshTokens.Add(refreshToken);

                // remove old refresh tokens from user
                //RemoveOldRefreshTokens(newUser);

                //await UnitOfWork.Users.UpdateAsync(newUser);
                //await UnitOfWork.SaveAsync();
                DbContext.Users.Update(newUser);
                await DbContext.SaveChangesAsync();

                var authenticateResponse = new AuthenticateResponse(newUser, newUserInfo, jwtToken, refreshToken.Token);

                result.WithSuccess(successMessage: string.Format(Resources.Messages.Successes.SuccessInsert, "User"));
                result.WithValue(value: authenticateResponse);

                //SetTokenCookie(refreshToken.Token);

                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.WithError(errorMessage: ex.Message);
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    result.WithError(ex.InnerException.Message);
                }
            }
            return result.ConvertToDtxResult();
        }

        [PermissionAuthorize(Core.Config.PermissionsConfig.Account.CanRead)]
        [HttpGet("get-all-user")]
        [ProducesResponseType(type: typeof(Result<IList<UserListViewModel>>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<Result<IList<UserListViewModel>>> GetAllUser()
        {
            var result = new FluentResults.Result<IList<UserListViewModel>>();
            try
            {
                //var userList = await QueryUnitOfWork.Users.GetAllAsync();
                var userList = await DbContext.Users.ToListAsync();
                var returnList =
                    new List<UserListViewModel>();
                foreach (var user in userList)
                {
                    var userInfo = await DbContext.UserInformations.SingleOrDefaultAsync(u => u.UserId == user.Id);
                    //var userInfo = await QueryUnitOfWork.UserInformations.GetUserInfoByUserIdAsync(user.Id);
                    returnList.Add(new UserListViewModel()
                    {
                        RegisterDateTime = user.RegisterDateTime,
                        Username = user.Username,
                        IsActive = user.IsActive,
                        Id = user.Id,
                        Birthdate = userInfo?.Birthdate,
                        Gender = userInfo?.Gender,
                        FirstName = userInfo != null ? userInfo.FirstName : "",
                        LastName = userInfo != null ? userInfo.LastName : "",
                        NationalId = userInfo != null ? userInfo.NationalId : ""
                    });
                }

                result.WithValue(value: returnList);
            }
            catch (Exception ex)
            {
                result.WithError(errorMessage: ex.Message);
            }
            //System.Threading.Thread.Sleep(3000);
            return result.ConvertToDtxResult();
        }

        //[PermissionAuthorize(Core.Config.PermissionsConfig.Account.CanRead)]
        [AllowAnonymous]
        [HttpGet("{id}/refresh-token")]
        [ProducesResponseType(type: typeof(Result<IList<UserListViewModel>>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<Result<IList<RefreshToken>>> GetRefreshTokens(Guid id)
        {
            var result = new FluentResults.Result<IList<RefreshToken>>();
            try
            {
                //var user = await QueryUnitOfWork.Users.GetUserByUserIdAsync(id);
                var user = await DbContext.Users.SingleOrDefaultAsync(u => u.Id == id);

                result.WithSuccess("Ok");
                result.WithValue(value: user.RefreshTokens);
            }
            catch (Exception ex)
            {
                result.WithError(errorMessage: ex.Message);
            }
            return result.ConvertToDtxResult();
        }

        [PermissionAuthorize(Core.Config.PermissionsConfig.Account.CanRead)]
        //[AllowAnonymous]
        [HttpGet("get-user/{id}")]
        [ProducesResponseType(type: typeof(Result<UserListViewModel>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<Result<UserListViewModel>> GetById(Guid id)
        {
            var result = new FluentResults.Result<UserListViewModel>();
            try
            {
                //var user = await QueryUnitOfWork.Users.GetUserByUserIdAsync(id);
                var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    result.WithError(errorMessage: "User Not Found!");
                    return result.ConvertToDtxResult();
                }
                //var userInfo = await QueryUnitOfWork.UserInformations.GetUserInfoByUserIdAsync(user.Id);
                var userInfo = await DbContext.UserInformations.SingleOrDefaultAsync(u => u.UserId == user.Id);

                var userListViewModel = new UserListViewModel()
                {
                    RegisterDateTime = user.RegisterDateTime,
                    Username = user.Username,
                    IsActive = user.IsActive,
                    Id = user.Id,
                    Birthdate = userInfo?.Birthdate,
                    Gender = userInfo?.Gender,
                    FirstName = userInfo != null ? userInfo.FirstName : "",
                    LastName = userInfo != null ? userInfo.LastName : "",
                    NationalId = userInfo != null ? userInfo.NationalId : "",
                    Bio = userInfo != null ? userInfo.Bio :"",
                    Certifications = userInfo != null ? userInfo.Certifications : "",
                    Email = userInfo != null ? userInfo.Email : "",
                    Equipment = userInfo != null ? userInfo.Equipment : "",
                    Language = userInfo != null ? userInfo.Language : "",
                    Phone = userInfo != null ? userInfo.Phone : "",
                    SocialMediaLinks = userInfo != null ? userInfo.SocialMediaLinks : ""
                };

                result.WithSuccess("Ok");
                result.WithValue(value: userListViewModel);
            }
            catch (Exception ex)
            {
                result.WithError(errorMessage: ex.Message);
            }
            return result.ConvertToDtxResult();
        }

        [PermissionAuthorize(Core.Config.PermissionsConfig.Account.CanRead)]
        [HttpPost("profile")]
        [ProducesResponseType(type: typeof(Result<AuthenticateResponse>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<Result> ProfileUpdate([FromBody] ProfileViewModel profileData)
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

                UserInformation userInformation = await DbContext.UserInformations.SingleOrDefaultAsync(u => u.UserId == profileData.UserId);
                if (userInformation == null)
                {
                    result.WithError("User Information Not found!");
                    return result.ConvertToDtxResult();
                }
                userInformation.Birthdate = profileData.Birthdate;
                userInformation.FirstName = profileData.FirstName;
                userInformation.LastName = profileData.LastName;
                userInformation.Email = profileData.Email;
                userInformation.Phone = profileData.Phone;
                userInformation.Bio = profileData.Bio;
                userInformation.Certifications = profileData.Certifications;
                userInformation.Gender = profileData.Gender;
                userInformation.Language = profileData.Language;
                userInformation.SocialMediaLinks = profileData.SocialMediaLinks;
                userInformation.Equipment = profileData.Equipment;

                DbContext.UserInformations.Update(userInformation);
                await DbContext.SaveChangesAsync();

                result.WithSuccess(successMessage: "Profile updated successfully.");
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

        [PermissionAuthorize]
        [HttpGet("test")]
        public string Test()
        {
            return "Hello You have the right access to the Test method.";
        }

        #endregion

        #region Helper Methods

        private async Task<User> GetUserAsync(string username)
        {
            //return await UnitOfWork.Users.GetByUsernameAsync(username.ToLower());
            return await DbContext.Users.SingleOrDefaultAsync(u => u.Username == username.ToLower());
        }

        /*
        private void SetTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        */

        private string IpAddress()
        {
            // get source ip address for the current request

            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];

            return HttpContext.Connection.RemoteIpAddress != null ?
                HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() :
                "localhost";
        }

        private void RemoveOldRefreshTokens(User user)
        {
            var listOldRefreshToken = _accountService.RemoveOldRefreshTokens(user);
            foreach (var oldRefreshToken in listOldRefreshToken)
            {
                //UnitOfWork.RefreshTokens.DeleteAsync(oldRefreshToken);
                DbContext.RefreshTokens.Remove(oldRefreshToken);
                user.RefreshTokens.Remove(oldRefreshToken);
            }
        }

        #endregion

        #region FakeData

        [AllowAnonymous]
        [HttpGet("generate-fake-data")]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<Result> GenerateFakeData()
        {
            var result = new FluentResults.Result();
            try
            {
                //*************************  Permission

                Permission p1 = new Permission { Name = "Permission.Account.CanEdit" };
                //await UnitOfWork.Permissions.InsertAsync(p1);
                await DbContext.Permissions.AddAsync(p1);

                Permission p2 = new Permission { Name = "Permission.Account.CanDelete" };
                //await UnitOfWork.Permissions.InsertAsync(p2);
                await DbContext.Permissions.AddAsync(p2);

                Permission p3 = new Permission { Name = "Permission.Action.FullAccess" };
                //await UnitOfWork.Permissions.InsertAsync(p3);
                await DbContext.Permissions.AddAsync(p3);

                Permission p4 = new Permission { Name = "Permission.Account.CanRead" };
                //await UnitOfWork.Permissions.InsertAsync(p4);
                await DbContext.Permissions.AddAsync(p4);

                Permission p5 = new Permission { Name = "Permission.Account.CanAdd" };
                //await UnitOfWork.Permissions.InsertAsync(p5);
                await DbContext.Permissions.AddAsync(p5);

                //*************************  Role

                Role r1 = new Role { Name = "Admin", };
                //await UnitOfWork.Roles.InsertAsync(r1);
                await DbContext.Roles.AddAsync(r1);

                Role r2 = new Role { Name = "User", };
                //await UnitOfWork.Roles.InsertAsync(r2);
                await DbContext.Roles.AddAsync(r2);

                //*************************  RolePermission

                RolePermission rp1 = new RolePermission { Role = r2, Permission = p4 };
                //await UnitOfWork.RolePermissions.InsertAsync(rp1);
                await DbContext.RolePermissions.AddAsync(rp1);

                RolePermission rp2 = new RolePermission { Role = r1, Permission = p3 };
                //await UnitOfWork.RolePermissions.InsertAsync(rp2);
                await DbContext.RolePermissions.AddAsync(rp2);

                //*************************  User & UserInformation

                var pd1 = _accountService.CreatePassword("1234");
                var u1 = new User
                {
                    Username = "admin@gili.com",
                    PasswordHash = pd1.PasswordHash,
                    PasswordSalt = pd1.PasswordSalt
                };
                //await UnitOfWork.Users.InsertAsync(u1);
                await DbContext.Users.AddAsync(u1);

                var uif1 = new UserInformation
                {
                    Birthdate = new DateTime(year: 1980, month: 01, day: 01),
                    FirstName = "Admin",
                    LastName = "Admin",
                    Gender = Gender.Man,
                    NationalId = "111111111",
                    User = u1
                };
                //await UnitOfWork.UserInformations.InsertAsync(uif1);
                await DbContext.UserInformations.AddAsync(uif1);

                var pd2 = _accountService.CreatePassword("1234");
                var u2 = new User
                {
                    Username = "amin.eisapour@gmail.com",
                    PasswordHash = pd2.PasswordHash,
                    PasswordSalt = pd2.PasswordSalt
                };
                //await UnitOfWork.Users.InsertAsync(u2);
                await DbContext.Users.AddAsync(u2);

                var uif2 = new UserInformation
                {
                    Birthdate = new DateTime(year: 1982, month: 07, day: 21),
                    FirstName = "Amin",
                    LastName = "Eisapour",
                    Gender = Gender.Man,
                    NationalId = "0068272618",
                    User = u2
                };
                //await UnitOfWork.UserInformations.InsertAsync(uif2);
                await DbContext.UserInformations.AddAsync(uif2);

                //*************************  UserRole

                var ur1 = new UserRole { User = u1, Role = r1 };
                //await UnitOfWork.UserRoles.InsertAsync(ur1)
                await DbContext.UserRoles.AddAsync(ur1);

                var ur2 = new UserRole { User = u2, Role = r2 };
                //await UnitOfWork.UserRoles.InsertAsync(ur2);
                await DbContext.UserRoles.AddAsync(ur2);


                //*************************  Save Data
                //await UnitOfWork.SaveAsync();
                await DbContext.SaveChangesAsync();

                result.WithSuccess("All data has been saved successfully.");
            }
            catch (Exception ex)
            {
                result.WithError(errorMessage: ex.Message);
            }
            return result.ConvertToDtxResult();
        }

        #endregion
    }
}
