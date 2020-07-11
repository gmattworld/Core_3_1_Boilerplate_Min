using APP.Core.Enums;
using APP.Core.Factories;
using APP.Core.Model;
using APP.Core.Model.ModelExt;
using APP.Core.Utilities;
using APP.Repository.EFRepo.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APP.API.Controllers.v1
{
    /// <summary>
    /// Profile Management
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
    [Authorize(AuthenticationSchemes = "OAuth")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Profile management constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="unitOfWork"></param>
        public ProfilesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Search country
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Get /api/v1.0/Profiles
        /// 
        /// </remarks>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="status"></param>
        /// <returns code="200"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponseExt<ProfileModel>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public ActionResult<DataResponseExt<ProfileModel>> Get(string name = null, string email = null, RecordStatus? status = null)
        {
            DataResponseExt<ProfileModel> response = new DataResponseExt<ProfileModel>();
            List<ProfileModel> _DTO = null;

            try
            {
                var _countries = _unitOfWork.Profiles.Search(name, email, null, null, null, status);
                _DTO = _mapper.Map<List<ProfileModel>>(_countries);

                response.Code = ResponseCode.SUCCESS;
                response.Description = ResponseDescription.SUCCESS;
                response.Message = null;
                response.Data = _DTO;
                return Ok(response);
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
        /// Get country by country ID
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Get /api/v1.0/Profiles/b41229e2-370d-4aa2-a6fc-7ded5926d4d0
        /// 
        /// </remarks>
        /// <param name="ID"></param>
        /// <returns code="200"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        /// <response code="404">Entity not found</response>
        [HttpGet("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<ProfileExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<ProfileExtModel>), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<ProfileExtModel>>> GetByID([FromRoute] string ID)
        {
            DataResponse<ProfileExtModel> response = new DataResponse<ProfileExtModel>();
            ProfileExtModel _DTO = null;

            try
            {
                var _country = await _unitOfWork.Profiles.GetAsync(ID).ConfigureAwait(false);
                if (_country == null)
                {
                    response.Code = ResponseCode.NOT_FOUND;
                    response.Description = ResponseDescription.NOT_FOUND;
                    response.Message = "Profile not found";
                    response.Data = _DTO;
                    return NotFound(response);
                }

                _DTO = _mapper.Map<ProfileExtModel>(_country);

                response.Code = ResponseCode.SUCCESS;
                response.Description = ResponseDescription.SUCCESS;
                response.Message = null;
                response.Data = _DTO;
                return Ok(response);
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


        ///// <summary>
        ///// Create new country
        ///// </summary>
        ///// <remarks>
        ///// URI:
        ///// 
        /////     Post /api/v1.0/Profiles
        ///// 
        ///// Payload:
        ///// 
        /////     {
        /////         "code": "string",
        /////         "name": "string"
        /////     }
        ///// 
        ///// </remarks>
        ///// <param name="model"></param>
        ///// <returns code="201"></returns>
        ///// <response code="400">Bad request</response>
        ///// <response code="401">User not authorized</response>
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(typeof(DataResponse<ProfileExtModel>), StatusCodes.Status400BadRequest)]
        //[ProducesDefaultResponseType]
        //public async Task<ActionResult<DataResponse<ProfileExtModel>>> Post([FromBody] ProfileMiniModel model)
        //{
        //    DataResponse<ProfileExtModel> response = new DataResponse<ProfileExtModel>();
        //    ProfileExtModel _DTO = null;
        //    string errorMsg = String.Empty;

        //    try
        //    {
        //        if (ModelState.IsValid && model != null)
        //        {
        //            // Custom Validations
        //            if (!ProfileFactory.ValidateModel(model, out errorMsg))
        //            {
        //                response.Code = ResponseCode.FAILED;
        //                response.Description = ResponseDescription.FAILED;
        //                response.Message = errorMsg;
        //                response.Data = _DTO;
        //                return BadRequest(response);
        //            }

        //            // Check Entity Exist
        //            if (_unitOfWork.Profiles.CheckExist(model.Name, model.Code, out errorMsg))
        //            {
        //                response.Code = ResponseCode.FAILED;
        //                response.Description = ResponseDescription.FAILED;
        //                response.Message = errorMsg;
        //                response.Data = _DTO;
        //                return BadRequest(response);
        //            }

        //            // Generate entity
        //            var _entity = ProfileFactory.Create(model);

        //            // Create user
        //            var _country = await _unitOfWork.Profiles.InsertAsync(_entity).ConfigureAwait(false);
        //            int done = await _unitOfWork.CompleteAsync().ConfigureAwait(false);

        //            if (done > 0)
        //            {
        //                _DTO = _mapper.Map<ProfileExtModel>(_country);

        //                response.Code = ResponseCode.SUCCESS;
        //                response.Description = ResponseDescription.SUCCESS;
        //                response.Message = null;
        //                response.Data = _DTO;
        //                return Created(new Uri($"{Request.Path}/{_DTO.ID}", UriKind.Relative), _DTO);
        //            }

        //        }

        //        response.Code = ResponseCode.FAILED;
        //        response.Description = ResponseDescription.FAILED;
        //        response.Message = "Invalid user input";
        //        response.Data = _DTO;
        //        return BadRequest(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        Guid _ErrorCode = Guid.NewGuid();
        //        Log.Error("Fatal Error [{ErrorCode}]: {Message}", _ErrorCode, ex.Message);
        //        response.Code = ResponseCode.SYSTEM_ERROR;
        //        response.Description = ResponseDescription.SYSTEM_ERROR;
        //        response.Message = $"System Error: Something went wrong here! Kindly contact the support with this error code [{_ErrorCode}]";
        //        response.Data = _DTO;
        //        return BadRequest(response);
        //    }
        //}



        /// <summary>
        /// Update country
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Post /api/v1.0/Profiles/b41229e2-370d-4aa2-a6fc-7ded5926d4d0
        /// 
        /// Payload:
        /// 
        ///     {
        ///         "code": "string",
        ///         "name": "string",
        ///         "isRestricted": true,
        ///         "id": "string",
        ///         "recordStatus": 0,
        ///         "isApproved": true
        ///     }
        /// 
        /// </remarks>
        /// <param name="model"></param>
        /// <param name="ID"></param>
        /// <returns code="201"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        [HttpPut("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<ProfileExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<ProfileExtModel>>> Put([FromBody] ProfileModel model, [FromRoute] string ID)
        {
            DataResponse<ProfileExtModel> response = new DataResponse<ProfileExtModel>();
            ProfileExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (ModelState.IsValid && model != null)
                {
                    // Custom Validations
                    if (!ProfileFactory.ValidateModel(model, ID, out errorMsg))
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    // Check Entity Exist
                    //if (_unitOfWork.Profiles.CheckExist(model.Name, model.Code, out errorMsg, ID))
                    //{
                    //    response.Code = ResponseCode.FAILED;
                    //    response.Description = ResponseDescription.FAILED;
                    //    response.Message = errorMsg;
                    //    response.Data = _DTO;
                    //    return BadRequest(response);
                    //}

                    // Generate entity
                    var _entity = ProfileFactory.Create(model, null, null);

                    // Create user
                    var _country = await _unitOfWork.Profiles.UpdateAsync(_entity).ConfigureAwait(false);
                    int done = await _unitOfWork.CompleteAsync().ConfigureAwait(false);

                    if (done > 0)
                    {
                        _DTO = _mapper.Map<ProfileExtModel>(_country);

                        response.Code = ResponseCode.SUCCESS;
                        response.Description = ResponseDescription.SUCCESS;
                        response.Message = null;
                        response.Data = _DTO;
                        return Ok(_DTO);
                    }
                }

                response.Code = ResponseCode.FAILED;
                response.Description = ResponseDescription.FAILED;
                response.Message = "Invalid user input";
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
        /// Delete a country
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Delete /api/v1.0/Profiles/b41229e2-370d-4aa2-a6fc-7ded5926d4d0
        /// 
        /// </remarks>
        /// <param name="ID"></param>
        /// <returns code="204"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        /// <response code="404">Entity not found</response>
        [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(DataResponse<ProfileExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<ProfileExtModel>), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<ProfileExtModel>>> Delete([FromRoute] string ID)
        {
            DataResponse<ProfileExtModel> response = new DataResponse<ProfileExtModel>();
            ProfileExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (ID != null)
                {
                    // Update country
                    var _country = await _unitOfWork.Profiles.DeleteProfile(ID).ConfigureAwait(false);
                    if (_country == null)
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return NotFound(response);
                    }

                    int done = await _unitOfWork.CompleteAsync().ConfigureAwait(false);

                    if (done > 0)
                    {
                        //_DTO = _mapper.Map<ProfileExtModel>(_country);

                        response.Code = ResponseCode.SUCCESS;
                        response.Description = ResponseDescription.SUCCESS;
                        response.Message = "Profile deleted";
                        response.Data = _DTO;
                        return NoContent();
                    }
                }

                response.Code = ResponseCode.FAILED;
                response.Description = ResponseDescription.FAILED;
                response.Message = "Invalid country ID";
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
    }
}