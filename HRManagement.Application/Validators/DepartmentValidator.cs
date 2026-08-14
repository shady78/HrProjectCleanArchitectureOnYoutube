using FluentValidation;
using HRManagement.Application.DTOs.Departments;

namespace HRManagement.Application.Validators
{
    public sealed class CreateDepartmentValidator 
        : AbstractValidator<CreateDepartmentRequest>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(request => request.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Department Name is requires.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");

            RuleFor(r => r.Locaiton)
                .MaximumLength(100)
                .WithMessage("Location must not exceed 100 characters.");
        }
    }
    public sealed class UpdateDepartmentValidator 
        : AbstractValidator<UpdateDepartmentRequest>
    {
        public UpdateDepartmentValidator()
        {
            RuleFor(request => request.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Department Name is requires.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");

            RuleFor(r => r.Locaiton)
                .MaximumLength(100)
                .WithMessage("Location must not exceed 100 characters.");
        }
    }
}
