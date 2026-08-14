using FluentValidation;

namespace HRManagement.Application.Validators
{
    public sealed class CreateJobTitleValidator
        : AbstractValidator<CreateJobTitleRequest>
    {
        public CreateJobTitleValidator()
        {
            RuleFor(t => t.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(100)
                .WithMessage("Title must not exceed 100 characters.");

            RuleFor(t => t.Description)
                .MaximumLength(100)
                .WithMessage("Description must not exceed 100 characters.");
        }
    }
    public sealed class UpdateJobTitleValidator
        : AbstractValidator<UpdateJobTitleRequest>
    {
        public UpdateJobTitleValidator()
        {
            RuleFor(t => t.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(100)
                .WithMessage("Title must not exceed 100 characters.");

            RuleFor(t => t.Description)
                .MaximumLength(100)
                .WithMessage("Description must not exceed 100 characters.");
        }
    }
}
