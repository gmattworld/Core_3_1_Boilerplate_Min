using APP.Core.Entities;
using APP.Core.Enums;
using APP.Core.Factories;
using APP.Core.Model;
using APP.Core.Model.ModelExt;
using APP.Core.Model.ModelMini;
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
    /// State Management
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
    public class StatesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// State management constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="unitOfWork"></param>
        public StatesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Search state
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Get /api/v1.0/States
        /// 
        /// </remarks>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="countryId"></param>
        /// <param name="status"></param>
        /// <returns code="200"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponseExt<StateModel>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponseExt<StateModel>>> GetAsync(string name = null, string code = null, string countryId = null, RecordStatus? status = null)
        {
            DataResponseExt<StateModel> response = new DataResponseExt<StateModel>();
            List<StateModel> _DTO = null;

            try
            {
                Country _theCountry = null;
                if (countryId != null)
                {
                    _theCountry = await _unitOfWork.Countries.GetAsync(countryId).ConfigureAwait(false);
                }

                var _states = _unitOfWork.States.Search(name, code, _theCountry, status);
                _DTO = _mapper.Map<List<StateModel>>(_states);

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
        /// Get state by state ID
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Get /api/v1.0/States/b41229e2-370d-4aa2-a6fc-7ded5926d4d0
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <returns code="200"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        /// <response code="404">Entity not found</response>
        [HttpGet("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<StateExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<StateExtModel>), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<StateExtModel>>> GetByID([FromRoute] string id)
        {
            DataResponse<StateExtModel> response = new DataResponse<StateExtModel>();
            StateExtModel _DTO = null;

            try
            {
                var _state = await _unitOfWork.States.GetAsync(id).ConfigureAwait(false);
                if (_state == null)
                {
                    response.Code = ResponseCode.NOT_FOUND;
                    response.Description = ResponseDescription.NOT_FOUND;
                    response.Message = "state not found";
                    response.Data = _DTO;
                    return NotFound(response);
                }

                _DTO = _mapper.Map<StateExtModel>(_state);

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
        /// Create new state
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Post /api/v1.0/States
        /// 
        /// Payload:
        /// 
        ///     {
        ///         "code": "string",
        ///         "name": "string",
        ///         "countryId": "string"
        ///     }
        /// 
        /// </remarks>
        /// <param name="model"></param>
        /// <returns code="201"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(DataResponse<StateExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<StateExtModel>>> Post([FromBody] StateMiniModel model)
        {
            DataResponse<StateExtModel> response = new DataResponse<StateExtModel>();
            StateExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (ModelState.IsValid && model != null)
                {
                    // Custom Validations
                    if (!StateFactory.ValidateModel(model, out errorMsg))
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    Country _theCountry = await _unitOfWork.Countries.GetAsync(model.CountryId).ConfigureAwait(false);

                    // Check Entity Exist
                    if (_unitOfWork.States.CheckExist(model.Name, model.Code, _theCountry, out errorMsg))
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    // Generate entity
                    var _entity = StateFactory.Create(model, _theCountry);

                    // Create user
                    var _state = await _unitOfWork.States.InsertAsync(_entity).ConfigureAwait(false);
                    int done = await _unitOfWork.CompleteAsync().ConfigureAwait(false);

                    if (done > 0)
                    {
                        _DTO = _mapper.Map<StateExtModel>(_state);

                        response.Code = ResponseCode.SUCCESS;
                        response.Description = ResponseDescription.SUCCESS;
                        response.Message = null;
                        response.Data = _DTO;
                        return Created(new Uri($"{Request.Path}/{_DTO.Id}", UriKind.Relative), _DTO);
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
        /// Update state
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Post /api/v1.0/States/b41229e2-370d-4aa2-a6fc-7ded5926d4d0
        /// 
        /// Payload:
        /// 
        ///     {
        ///         "code": "string",
        ///         "name": "string",
        ///         "countryId": "string",
        ///         "isRestricted": true,
        ///         "id": "string",
        ///         "recordStatus": 0,
        ///         "isApproved": true
        ///     }
        /// 
        /// </remarks>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns code="201"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        [HttpPut("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<StateExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<StateExtModel>>> Put([FromBody] StateModel model, [FromRoute] string id)
        {
            DataResponse<StateExtModel> response = new DataResponse<StateExtModel>();
            StateExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (ModelState.IsValid && model != null)
                {
                    // Custom Validations
                    if (!StateFactory.ValidateModel(model, id, out errorMsg))
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    Country _theCountry = await _unitOfWork.Countries.GetAsync(model.CountryId).ConfigureAwait(false);

                    // Check Entity Exist
                    if (_unitOfWork.States.CheckExist(model.Name, model.Code, _theCountry, out errorMsg, id))
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    // Generate entity
                    var _entity = StateFactory.Create(model, _theCountry);

                    // Create user
                    var _state = await _unitOfWork.States.UpdateAsync(_entity).ConfigureAwait(false);
                    int done = await _unitOfWork.CompleteAsync().ConfigureAwait(false);

                    if (done > 0)
                    {
                        _DTO = _mapper.Map<StateExtModel>(_state);

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
        /// Restrict or remove restriction placed on a state
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Post /api/v1.0/States/b41229e2-370d-4aa2-a6fc-7ded5926d4d0/Restriction
        /// 
        /// Payload:
        /// 
        ///     {}
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <returns code="200"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        /// <response code="404">Entity not found</response>
        [HttpPatch("{ID}/Restriction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<StateExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<StateExtModel>), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<StateExtModel>>> Patch([FromRoute] string id)
        {
            DataResponse<StateExtModel> response = new DataResponse<StateExtModel>();
            StateExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (id != null)
                {
                    // Update state
                    var _state = await _unitOfWork.States.UpdateStateRestrictions(id).ConfigureAwait(false);
                    if (_state == null)
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
                        _DTO = _mapper.Map<StateExtModel>(_state);

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
        /// Delete a state
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Delete /api/v1.0/States/b41229e2-370d-4aa2-a6fc-7ded5926d4d0
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <returns code="204"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        /// <response code="404">Entity not found</response>
        [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(DataResponse<StateExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<StateExtModel>), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<StateExtModel>>> Delete([FromRoute] string id)
        {
            DataResponse<StateExtModel> response = new DataResponse<StateExtModel>();
            StateExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (id != null)
                {
                    // Update state
                    var _state = await _unitOfWork.States.DeleteState(id).ConfigureAwait(false);
                    if (_state == null)
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
                        response.Code = ResponseCode.SUCCESS;
                        response.Description = ResponseDescription.SUCCESS;
                        response.Message = "state deleted";
                        response.Data = _DTO;
                        return NoContent();
                    }
                }

                response.Code = ResponseCode.FAILED;
                response.Description = ResponseDescription.FAILED;
                response.Message = "Invalid state ID";
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