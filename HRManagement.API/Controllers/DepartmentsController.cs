using HRManagement.API.Common.Responses;
using HRManagement.Application.Common;
using HRManagement.Application.DTOs.Departments;
using HRManagement.Application.Services.Departments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController(IDepartmentService service) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellactionToken)
        {
            var departments = await service.GetAllAsync(cancellactionToken);
            return Ok(ApiResponse<IReadOnlyList<DepartmentResponse>>
                .Succeeded(departments, "Departments retrieved successfully."));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await service.GetByIdAsync(id, cancellationToken);
            return Ok(
                ApiResponse<DepartmentResponse>
                .Succeeded(result.Value!, "Department retrieved successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentRequest request, CancellationToken cancellationToken)
        {
            var result = await service.CreateAsync(request, cancellationToken);
            if (!result.IsSuccess)
            {
                return ToErrorResponse(result.Error!);
            }

            var response = ApiResponse<DepartmentResponse>
                .Succeeded(result.Value!, "Department created successfully.");

            return CreatedAtAction
                  (nameof(GetById),
                  new { id = result.Value!.Id },
                 response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateDepartmentRequest request,CancellationToken cancellationToken)
        {
            var result = await service.UpdateAsync(id, request, cancellationToken);

           var response = ApiResponse<DepartmentResponse>
                .Succeeded(result.Value!, "Department updated successfully.");
           
            return result.IsSuccess ? Ok(response) : ToErrorResponse(result.Error!);

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await service.DeactivateAsync(id, cancellationToken);
            if (!result.IsSuccess)
            {
                return ToErrorResponse(result.Error!);
            }
            return result.IsSuccess
                ? NoContent() : ToErrorResponse(result.Error!);
        }
        private ObjectResult ToErrorResponse(ServiceError error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status400BadRequest
            };

            var response = ApiResponse<object?>.Failed(
                error.Message,
                new[]
                {
                    new
                    {
                        error.Code,
                        error.Message
                    }
                });

            return StatusCode(statusCode, response);
        }
    }
}
