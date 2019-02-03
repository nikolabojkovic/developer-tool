using System;
using FluentValidation;
using System.Linq;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Models;

namespace WebApi.InputModels {
    public class TodoValidator : AbstractValidator<TodoInputModel>
    {
        public TodoValidator()
        {
            RuleFor(m => m.Description).NotNull().WithMessage("Todo description is required");
        }
    }
}