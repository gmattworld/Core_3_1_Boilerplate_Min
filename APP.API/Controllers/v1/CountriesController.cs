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
    /// Country Management
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
    public class CountriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Country management constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="unitOfWork"></param>
        public CountriesController(IMapper mapper, IUnitOfWork unitOfWork)
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
        ///     Get /api/v1.0/Countries
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
        [ProducesResponseType(typeof(DataResponseExt<CountryModel>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public ActionResult<DataResponseExt<CountryModel>> Get(string name = null, string code = null, RecordStatus? status = null)
        {
            DataResponseExt<CountryModel> response = new DataResponseExt<CountryModel>();
            List<CountryModel> _DTO = null;

            try
            {
                List<Country> _countries = _unitOfWork.Countries.Search(name, code, status);
                _DTO = _mapper.Map<List<CountryModel>>(_countries);

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
        ///     Get /api/v1.0/Countries/b41229e2-370d-4aa2-a6fc-7ded5926d4d0
        /// 
        /// </remarks>
        /// <param name="ID"></param>
        /// <returns code="200"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        /// <response code="404">Entity not found</response>
        [HttpGet("{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<CountryExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<CountryExtModel>), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<CountryExtModel>>> GetByID([FromRoute] string ID)
        {
            DataResponse<CountryExtModel> response = new DataResponse<CountryExtModel>();
            CountryExtModel _DTO = null;

            try
            {
                var _country = await _unitOfWork.Countries.GetAsync(ID).ConfigureAwait(false);
                if (_country == null)
                {
                    response.Code = ResponseCode.NOT_FOUND;
                    response.Description = ResponseDescription.NOT_FOUND;
                    response.Message = "Country not found";
                    response.Data = _DTO;
                    return NotFound(response);
                }

                _DTO = _mapper.Map<CountryExtModel>(_country);

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
        /// Create new country
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Post /api/v1.0/Countries
        /// 
        /// Payload:
        /// 
        ///     {
        ///         "code": "string",
        ///         "name": "string"
        ///     }
        /// 
        /// </remarks>
        /// <param name="model"></param>
        /// <returns code="201"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(DataResponse<CountryExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<CountryExtModel>>> Post([FromBody] CountryMiniModel model)
        {
            DataResponse<CountryExtModel> response = new DataResponse<CountryExtModel>();
            CountryExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (ModelState.IsValid && model != null)
                {
                    // Custom Validations
                    if (!CountryFactory.ValidateModel(model, out errorMsg))
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    // Check Entity Exist
                    if (_unitOfWork.Countries.CheckExist(model.Name, model.Code, out errorMsg))
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    // Generate entity
                    var _entity = CountryFactory.Create(model);

                    // Create user
                    var _country = await _unitOfWork.Countries.InsertAsync(_entity).ConfigureAwait(false);
                    int done = await _unitOfWork.CompleteAsync().ConfigureAwait(false);

                    if (done > 0)
                    {
                        _DTO = _mapper.Map<CountryExtModel>(_country);

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
        /// Update country
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Post /api/v1.0/Countries/b41229e2-370d-4aa2-a6fc-7ded5926d4d0
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
        [ProducesResponseType(typeof(DataResponse<CountryExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<CountryExtModel>>> Put([FromBody] CountryModel model, [FromRoute] string ID)
        {
            DataResponse<CountryExtModel> response = new DataResponse<CountryExtModel>();
            CountryExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (ModelState.IsValid && model != null)
                {
                    // Custom Validations
                    if (!CountryFactory.ValidateModel(model, ID, out errorMsg))
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    // Check Entity Exist
                    if (_unitOfWork.Countries.CheckExist(model.Name, model.Code, out errorMsg, ID))
                    {
                        response.Code = ResponseCode.FAILED;
                        response.Description = ResponseDescription.FAILED;
                        response.Message = errorMsg;
                        response.Data = _DTO;
                        return BadRequest(response);
                    }

                    // Generate entity
                    var _entity = CountryFactory.Create(model);

                    // Create user
                    var _country = await _unitOfWork.Countries.UpdateAsync(_entity).ConfigureAwait(false);
                    int done = await _unitOfWork.CompleteAsync().ConfigureAwait(false);

                    if (done > 0)
                    {
                        _DTO = _mapper.Map<CountryExtModel>(_country);

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
        /// Restrict or remove restriction placed on a country
        /// </summary>
        /// <remarks>
        /// URI:
        /// 
        ///     Post /api/v1.0/Countries/b41229e2-370d-4aa2-a6fc-7ded5926d4d0/Restriction
        /// 
        /// Payload:
        /// 
        ///     {}
        /// 
        /// </remarks>
        /// <param name="ID"></param>
        /// <returns code="200"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        /// <response code="404">Entity not found</response>
        [HttpPatch("{ID}/Restriction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<CountryExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<CountryExtModel>), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<CountryExtModel>>> Patch([FromRoute] string ID)
        {
            DataResponse<CountryExtModel> response = new DataResponse<CountryExtModel>();
            CountryExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (ID != null)
                {
                    // Update country
                    var _country = await _unitOfWork.Countries.UpdateCountryRestrictions(ID).ConfigureAwait(false);
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
                        _DTO = _mapper.Map<CountryExtModel>(_country);

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
        ///     Delete /api/v1.0/Countries/b41229e2-370d-4aa2-a6fc-7ded5926d4d0
        /// 
        /// </remarks>
        /// <param name="ID"></param>
        /// <returns code="204"></returns>
        /// <response code="400">Bad request</response>
        /// <response code="401">User not authorized</response>
        /// <response code="404">Entity not found</response>
        [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(DataResponse<CountryExtModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<CountryExtModel>), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<DataResponse<CountryExtModel>>> Delete([FromRoute] string ID)
        {
            DataResponse<CountryExtModel> response = new DataResponse<CountryExtModel>();
            CountryExtModel _DTO = null;
            string errorMsg = String.Empty;

            try
            {
                if (ID != null)
                {
                    // Update country
                    var _country = await _unitOfWork.Countries.DeleteCountry(ID).ConfigureAwait(false);
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
                        //_DTO = _mapper.Map<CountryExtModel>(_country);

                        response.Code = ResponseCode.SUCCESS;
                        response.Description = ResponseDescription.SUCCESS;
                        response.Message = "Country deleted";
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