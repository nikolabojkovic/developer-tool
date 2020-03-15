using FluentValidation;

namespace WebApi.InputModels {
    public class TodoValidator : AbstractValidator<TodoInputModel>
    {
        public TodoValidator()
        {
            RuleFor(m => m.Description).NotNull().WithMessage("Todo description is required");
        }
    }
}