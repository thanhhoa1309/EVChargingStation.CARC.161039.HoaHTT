using EVChargingStation.CARC.Application.HoaHTT.Interfaces;
using EVChargingStation.CARC.Application.HoaHTT.Utils;
using EVChargingStation.CARC.Domain.HoaHTT.DTOs.PlanDTOs;
using EVChargingStation.CARC.Infrastructure.HoaHTT.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EVChargingStation.CARC.WebAPI.HoaHTT.Controllers
{
    namespace MovieTheater.API.Controllers
    {
        [Route("api/plans")]
        [ApiController]
        public class PlanController : ControllerBase
        {
            private readonly IPlanService _planService;

            public PlanController(IPlanService planService)
            {
                _planService = planService;
            }

            // ================================
            // GET: api/plans
            // ================================
            [HttpGet]
            [SwaggerOperation(
                Summary = "Get all plans",
                Description = "Retrieve a paginated list of plans with optional search, sorting, and status filtering.")]
            [ProducesResponseType(typeof(ApiResult<Pagination<PlanResponceDTOs>>), 200)]
            [ProducesResponseType(typeof(ApiResult<object>), 400)]
            [ProducesResponseType(typeof(ApiResult<object>), 500)]
            public async Task<IActionResult> GetAllPlansAsync(
                [FromQuery, SwaggerParameter(Description = "Search by plan name (optional)")] string? search,
                [FromQuery, SwaggerParameter(Description = "Sort by field: name, price, duration, etc. (optional)")] string? sortBy,
                [FromQuery, SwaggerParameter(Description = "Sort in descending order? Default: false")] bool isDescending = false,
                [FromQuery, SwaggerParameter(Description = "Page number, starting from 1")] int page = 1,
                [FromQuery, SwaggerParameter(Description = "Number of items per page")] int pageSize = 10)

            {
                try
                {
                    if (page < 1 || pageSize < 1)
                        return BadRequest(ApiResult<object>.Failure("400", "Invalid pagination parameters."));

                    var result = await _planService.GetAllPlansAsync(search, sortBy, isDescending, page, pageSize);

                    return Ok(ApiResult<Pagination<PlanResponceDTOs>>.Success(result, "200", "Plans retrieved successfully."));
                }
                catch (Exception ex)
                {
                    var statusCode = ExceptionUtils.ExtractStatusCode(ex);
                    var errorResponse = ExceptionUtils.CreateErrorResponse<object>(ex);
                    return StatusCode(statusCode, errorResponse);
                }
            }

            // ================================
            // GET: api/plans/{id}
            // ================================
            [HttpGet("{id}")]
            [SwaggerOperation(
                Summary = "Get plan details",
                Description = "Retrieve detailed information for a specific plan by its ID.")]
            [ProducesResponseType(typeof(ApiResult<PlanResponceDTOs>), 200)]
            [ProducesResponseType(typeof(ApiResult<object>), 404)]
            [ProducesResponseType(typeof(ApiResult<object>), 500)]
            public async Task<IActionResult> GetPlanDetailAsync([FromRoute] Guid id)
            {
                try
                {
                    var plan = await _planService.GetPlanByIdAsync(id);
                    return Ok(ApiResult<PlanResponceDTOs>.Success(plan, "200", "Plan details retrieved successfully."));
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(ApiResult<object>.Failure("404", ex.Message));
                }
                catch (Exception ex)
                {
                    var statusCode = ExceptionUtils.ExtractStatusCode(ex);
                    var errorResponse = ExceptionUtils.CreateErrorResponse<object>(ex);
                    return StatusCode(statusCode, errorResponse);
                }
            }

            // ================================
            // POST: api/plans
            // ================================
            [HttpPost]
            [Authorize(Policy = "AdminPolicy")]
            [SwaggerOperation(
                Summary = "Create a new plan",
                Description = "Add a new plan to the system. Requires Admin privileges.")]
            [ProducesResponseType(typeof(ApiResult<PlanResponceDTOs>), 200)]
            [ProducesResponseType(typeof(ApiResult<object>), 400)]
            [ProducesResponseType(typeof(ApiResult<object>), 500)]
            public async Task<IActionResult> CreatePlanAsync([FromBody] PlanRequestDTOs planCreateDto)
            {
                try
                {
                    if (planCreateDto == null)
                        return BadRequest(ApiResult<object>.Failure("400", "Plan data is required."));

                    var result = await _planService.CreatePlanAsync(planCreateDto);
                    return Ok(ApiResult<PlanResponceDTOs>.Success(result, "200", "Plan created successfully."));
                }
                catch (Exception ex)
                {
                    var statusCode = ExceptionUtils.ExtractStatusCode(ex);
                    var errorResponse = ExceptionUtils.CreateErrorResponse<object>(ex);
                    return StatusCode(statusCode, errorResponse);
                }
            }


            // ================================
            // PUT: api/plans/{id}
            // ================================
            [HttpPut("{id}")]
            [Authorize(Policy = "AdminPolicy")]
            [SwaggerOperation(
                Summary = "Update plan information",
                Description = "Updates the details of a specific plan by its ID.")]
            [ProducesResponseType(typeof(ApiResult<PlanUpdateDTOs>), 200)]
            [ProducesResponseType(typeof(ApiResult<object>), 400)]
            [ProducesResponseType(typeof(ApiResult<object>), 500)]
            public async Task<IActionResult> UpdatePlanAsync([FromRoute] Guid id, [FromBody] PlanUpdateDTOs planUpdateDto)
            {
                try
                {
                    if (planUpdateDto == null)
                        return BadRequest(ApiResult<object>.Failure("400", "Plan update data is required."));

                    var updatedPlan = await _planService.UpdatePlanAsync(id, planUpdateDto);
                    return Ok(ApiResult<PlanUpdateDTOs>.Success(updatedPlan, "200", "Plan updated successfully."));
                }
                catch (Exception ex)
                {
                    var statusCode = ExceptionUtils.ExtractStatusCode(ex);
                    var errorResponse = ExceptionUtils.CreateErrorResponse<object>(ex);
                    return StatusCode(statusCode, errorResponse);
                }
            }

            // ================================
            // DELETE: api/plans/{id}
            // ================================
            [HttpDelete("{id}")]
            [Authorize(Policy = "AdminPolicy")]
            [SwaggerOperation(
                Summary = "Delete plan",
                Description = "Deletes a specific plan by its ID. Requires Admin privileges.")]
            [ProducesResponseType(typeof(ApiResult<bool>), 200)]
            [ProducesResponseType(typeof(ApiResult<object>), 404)]
            [ProducesResponseType(typeof(ApiResult<object>), 500)]
            public async Task<IActionResult> DeletePlanAsync(Guid id)
            {
                try
                {
                    var result = await _planService.DeletePlanAsync(id);

                    if (!result)
                        return NotFound(ApiResult<object>.Failure("404", $"Plan with ID {id} not found."));

                    return Ok(ApiResult<bool>.Success(result, "200", "Plan deleted successfully."));
                }
                catch (Exception ex)
                {
                    var statusCode = ExceptionUtils.ExtractStatusCode(ex);
                    var errorResponse = ExceptionUtils.CreateErrorResponse<object>(ex);
                    return StatusCode(statusCode, errorResponse);
                }
            }
        }
    }
}
