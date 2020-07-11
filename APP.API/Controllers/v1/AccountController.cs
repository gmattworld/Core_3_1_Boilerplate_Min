using APP.API.Utilities;
using APP.Core.Model;
using APP.Core.Model.ModelExt;
using APP.Core.Utilities;
using APP.Repository.EFRepo.EntitiesExt;
using APP.Services.Email;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace APP.API.Controllers.v1
{
    /// <summary>
    /// Account Management
    /// </summary>
    /// <remarks>
    /// Author:
    /// 
    ///     Afolabi Gmatt Matthew
    ///     
    /// Copyright:
    /// 
    ///     Copyright (c) Phareztech Code Lab. All rights reserved.
    /// 
    /// </remarks>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<IUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Account Management Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="config"></param>
        /// <param name="mapper"></param>
        /// <param name="emailService"></param>
        public AccountController(UserManager<IUser> userManager, IConfiguration config, IMapper mapper, IEmailService emailService)
        {
            _config = config;
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
        }

        /// <summary>
        /// User login endpoint
        /// </summary>
        /// <param name="model"></param>
        /// <returns code="200"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<UsersExtModel_Auth>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<UsersExtModel_Auth>), StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<UsersExtModel_Auth>>> AuthenticateAsync([FromBody] LoginModel model)
        {
            DataResponse<UsersExtModel_Auth> response = new DataResponse<UsersExtModel_Auth>();
            UsersExtModel_Auth _DTO = null;

            try
            {
                if (ModelState.IsValid && model != null)
                {
                    var _user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
                    if (_user == null)
                    {
                        response.Code = ResponseCode.LOGIN_FAILED;
                        response.Description = ResponseDescription.LOGIN_FAILED;
                        response.Message = "Invalid login credential";
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    var _signInRes = _userManager.PasswordHasher.VerifyHashedPassword(_user, _user.PasswordHash, model.Password);
                    if (_signInRes == PasswordVerificationResult.Success)
                    {
                        _DTO = _mapper.Map<UsersExtModel_Auth>(_user);
                        if (_DTO != null) _DTO = _DTO.GenerateToken(_config);

                        response.Code = ResponseCode.SUCCESS;
                        response.Description = ResponseDescription.SUCCESS;
                        response.Message = null;
                        response.Data = _DTO;
                        return Ok(_DTO);
                    }
                }

                response.Code = ResponseCode.LOGIN_FAILED;
                response.Description = ResponseDescription.LOGIN_FAILED;
                response.Message = "Invalid login credential";
                response.Data = _DTO;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                Guid _ErrorCode = Guid.NewGuid();
                Log.Error("Fatal Error [{ErrorCode}]: {Message}", _ErrorCode, ex.Message);
                response.Code = ResponseCode.SYSTEM_ERROR;
                response.Description = ResponseDescription.SYSTEM_ERROR;
                response.Message = $"System Error: Something went wrong here! Kindly contact the support with this error code [{_ErrorCode}]";
                response.Data = _DTO;
                return BadRequest(response);
            }
        }


        /// <summary>
        /// User registration endpoint
        /// </summary>
        /// <param name="model"></param>
        /// <returns code="200"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(DataResponse<UsersExtModel_Auth>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<UsersExtModel_Auth>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<UsersExtModel_Auth>>> RegisterAsync([FromBody] RegisterModel model)
        {
            DataResponse<UsersExtModel_Auth> response = new DataResponse<UsersExtModel_Auth>();
            UsersExtModel_Auth _DTO = null;

            try
            {
                if (ModelState.IsValid && model != null)
                {
                    var user = new IUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        NormalizedEmail = model.Email,
                        NormalizedUserName = model.UserName,
                        EmailConfirmed = false,
                        UserName = model.UserName,
                        Email = model.Email
                    };

                    var res = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
                    if (res.Succeeded)
                    {
                        // send mail to user
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);

                        await _emailService.Send(user.Email, subject: "Email Verification", body: $"Content: {code}").ConfigureAwait(false);

                        var _user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
                        if (_user != null)
                        {
                            var _signInRes = _userManager.PasswordHasher.VerifyHashedPassword(_user, _user.PasswordHash, model.Password);

                            if (_signInRes == PasswordVerificationResult.Success)
                            {
                                _DTO = _mapper.Map<UsersExtModel_Auth>(_user);
                                if (_DTO != null) _DTO = _DTO.GenerateToken(_config);

                                response.Code = ResponseCode.SUCCESS;
                                response.Description = ResponseDescription.SUCCESS;
                                response.Message = null;
                                response.Data = _DTO;
                                return Ok(_DTO);
                            }
                        }
                    }

                    response.Code = ResponseCode.LOGIN_FAILED;
                    response.Description = ResponseDescription.LOGIN_FAILED;
                    response.Message = "Invalid login credential";
                    response.Errors = res.Errors.Select(ex => ex.Description).ToList();
                    response.Data = _DTO;
                    return BadRequest(response);
                }
                else
                {
                    response.Code = ResponseCode.LOGIN_FAILED;
                    response.Description = ResponseDescription.LOGIN_FAILED;
                    response.Message = "Invalid login credential";
                    response.Data = _DTO;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                Guid _ErrorCode = Guid.NewGuid();
                Log.Error("Fatal Error [{ErrorCode}]: {Message}", _ErrorCode, ex.Message);
                response.Code = ResponseCode.SYSTEM_ERROR;
                response.Description = ResponseDescription.SYSTEM_ERROR;
                response.Message = $"System Error: Something went wrong here! Kindly contact the support with this error code [{_ErrorCode}]";
                response.Data = _DTO;
                return BadRequest(response);
            }
        }
    }
}