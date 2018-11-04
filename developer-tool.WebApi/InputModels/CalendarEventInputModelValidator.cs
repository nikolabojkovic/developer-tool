using System;
using FluentValidation;
using System.Linq;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Models;

namespace WebApi.InputModels {
    public class CalendarEventValidator : AbstractValidator<CalendarEventInputModel>
    {
        public CalendarEventValidator()
        {
            RuleFor(m => m.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(m => m.Title).MaximumLength(50);
            RuleFor(m => m.Start).Must(BeAValidDate).WithMessage("Start date is required");
            RuleFor(m => m.Reminder).SetValidator(new ReminderValidator()).When(m => m.Reminder != null);
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}