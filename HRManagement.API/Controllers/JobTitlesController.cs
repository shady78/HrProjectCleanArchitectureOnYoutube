using HRManagement.API.Common.Responses;
using HRManagement.Application.Common;
using HRManagement.Application.DTOs.JobTitles;
using HRManagement.Application.Services.JobTitles;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitlesController(IJobTitleService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var jobTitles = await service.GetAllAsync(cancellationToken);
            return Ok(ApiResponse<IReadOnlyList<JobTitleResponse>>.Succeeded(jobTitles,
                "Job titles retrieved successfuly."));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id,CancellationToken cancellationToken)
        {
            var result = await service.GetByIdAsync(id, cancellationToken);

            return result.IsSuccess
                ? Ok(ApiResponse<JobTitleResponse>.Succeeded(
                    result.Value!,
                    "Job title retrieved successfully."))
                : ToErrorResponse(result.Error!);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateJobTitleRequest request, CancellationToken cancellationToken)
        {
            var result = await service.CreateAsync(request, cancellationToken);
            if (!result.IsSuccess)
            {
                return ToErrorResponse(result.Error!);
            }
            var response = ApiResponse<JobTitleResponse>.Succeeded(result.Value!,
                "Job title created successfully.");

            return CreatedAtAction(
                nameof(Get),
                new { id = result.Value!.Id },
                response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UpdateJobTitleRequest request , CancellationToken cancellationToken)
        {
            var result = await service.UpdateAsync(id,request, cancellationToken);
            return result.IsSuccess
                ? Ok(ApiResponse<JobTitleResponse>.Succeeded(result.Value!,
                "Job Title Updated successfully."))
                : ToErrorResponse(result.Error!);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken)
        {
            var result = await service.DeactivateAsync(id, cancellationToken);
            return result.IsSuccess
                ? Ok(ApiResponse<object?>.Succeeded(
                    null,"Job title deactivated successfully."))
                : ToErrorResponse(result.Error!);
        }
        // helper method
        private ObjectResult ToErrorResponse(ServiceError error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status400BadRequest
            };

            return StatusCode(statusCode, ApiResponse<object>.Failed(error.Message,
                new[] { new { error.Code, error.Message } }));
        }
    }
}
