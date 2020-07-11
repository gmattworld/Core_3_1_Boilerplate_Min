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
    /// LGA Management
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
    public class LGAsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// LGA management constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="unitOfWork"></param>
        public LGAsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Search LGA
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Get /api/v1.0/LGAs
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
        [ProducesResponseType(typeof(DataResponseExt<LGAModel>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponseExt<LGAModel>>> GetAsync(string name = null, string code = null, string countryId = null, RecordStatus? status = null)
        {
            DataResponseExt<LGAModel> response = new DataResponseExt<LGAModel>();
            List<LGAModel> _DTO = null;

            try
            {
                State _theState = null;
                if (countryId != null)
                {
                    _theState = await _unitOfWork.States.GetAsync(countryId).ConfigureAwait(false);
                }

                var _LGAs = _unitOfWork.LGAs.Search(name, code, _theState, status);
                _DTO = _mapper.Map<List<LGAModel>>(_LGAs);

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
        /// Get LGA by LGA ID
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Get /api/v1.0/LGAs/b41229e2-370d-4aa2-a6fc-7ded5926d4d0
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <returns code="200"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        /// <response code="404">Entity not found</response>
        [HttpGet("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<LGAExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<LGAExtModel>), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<LGAExtModel>>> GetByID([FromRoute] string id)
        {
            DataResponse<LGAExtModel> response = new DataResponse<LGAExtModel>();
            LGAExtModel _DTO = null;

            try
            {
                var _LGA = await _unitOfWork.LGAs.GetAsync(id).ConfigureAwait(false);
                if (_LGA == null)
                {
                    response.Code = ResponseCode.NOT_FOUND;
                    response.Description = ResponseDescription.NOT_FOUND;
                    response.Message = "LGA not found";
                    response.Data = _DTO;
                    return NotFound(response);
                }

                _DTO = _mapper.Map<LGAExtModel>(_LGA);

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
        /// Create new LGA
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Post /api/v1.0/LGAs
        /// 
        /// Payload:
        /// 
        ///     {
        ///         "code": "string",
        ///         "name": "string",
        ///         "stateId": "string"
        ///     }
        /// 
        /// </remarks>
        /// <param name="model"></param>
        /// <returns code="201"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(DataResponse<LGAExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<LGAExtModel>>> Post([FromBody] LGAMiniModel model)
        {
            DataResponse<LGAExtModel> response = new DataResponse<LGAExtModel>();
            LGAExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (ModelState.IsValid && model != null)
                {
                    // Custom Validations
                    if (!LGAFactory.ValidateModel(model, out errorMsg))
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    State _theState = await _unitOfWork.States.GetAsync(model.StateId).ConfigureAwait(false);

                    // Check Entity Exist
                    if (_unitOfWork.LGAs.CheckExist(model.Name, model.Code, _theState, out errorMsg))
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    // Generate entity
                    var _entity = LGAFactory.Create(model, _theState);

                    // Create user
                    var _LGA = await _unitOfWork.LGAs.InsertAsync(_entity).ConfigureAwait(false);
                    int done = await _unitOfWork.CompleteAsync().ConfigureAwait(false);

                    if (done > 0)
                    {
                        _DTO = _mapper.Map<LGAExtModel>(_LGA);

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
        /// Update LGA
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Post /api/v1.0/LGAs/b41229e2-370d-4aa2-a6fc-7ded5926d4d0
        /// 
        /// Payload:
        /// 
        ///     {
        ///         "code": "string",
        ///         "name": "string",
        ///         "stateId": "string",
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
        [ProducesResponseType(typeof(DataResponse<LGAExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<LGAExtModel>>> Put([FromBody] LGAModel model, [FromRoute] string id)
        {
            DataResponse<LGAExtModel> response = new DataResponse<LGAExtModel>();
            LGAExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (ModelState.IsValid && model != null)
                {
                    // Custom Validations
                    if (!LGAFactory.ValidateModel(model, id, out errorMsg))
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    State _theState = await _unitOfWork.States.GetAsync(model.StateId).ConfigureAwait(false);

                    // Check Entity Exist
                    if (_unitOfWork.LGAs.CheckExist(model.Name, model.Code, _theState, out errorMsg, id))
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    // Generate entity
                    var _entity = LGAFactory.Create(model, _theState);

                    // Create user
                    var _LGA = await _unitOfWork.LGAs.UpdateAsync(_entity).ConfigureAwait(false);
                    int done = await _unitOfWork.CompleteAsync().ConfigureAwait(false);

                    if (done > 0)
                    {
                        _DTO = _mapper.Map<LGAExtModel>(_LGA);

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
        /// Restrict or remove restriction placed on a LGA
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Post /api/v1.0/LGAs/b41229e2-370d-4aa2-a6fc-7ded5926d4d0/Restriction
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
        [ProducesResponseType(typeof(DataResponse<LGAExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<LGAExtModel>), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<LGAExtModel>>> Patch([FromRoute] string id)
        {
            DataResponse<LGAExtModel> response = new DataResponse<LGAExtModel>();
            LGAExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (id != null)
                {
                    // Update LGA
                    var _LGA = await _unitOfWork.LGAs.UpdateLGARestrictions(id).ConfigureAwait(false);
                    if (_LGA == null)
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
                        _DTO = _mapper.Map<LGAExtModel>(_LGA);

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
        /// Delete a LGA
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Delete /api/v1.0/LGAs/b41229e2-370d-4aa2-a6fc-7ded5926d4d0
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <returns code="204"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        /// <response code="404">Entity not found</response>
        [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(DataResponse<LGAExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<LGAExtModel>), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<LGAExtModel>>> Delete([FromRoute] string id)
        {
            DataResponse<LGAExtModel> response = new DataResponse<LGAExtModel>();
            LGAExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (id != null)
                {
                    // Update LGA
                    var _LGA = await _unitOfWork.LGAs.DeleteLGA(id).ConfigureAwait(false);
                    if (_LGA == null)
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
                        response.Message = "LGA deleted";
                        response.Data = _DTO;
                        return NoContent();
                    }
                }

                response.Code = ResponseCode.FAILED;
                response.Description = ResponseDescription.FAILED;
                response.Message = "Invalid LGA ID";
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