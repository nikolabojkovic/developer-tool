using System;
using FluentValidation;
using System.Linq;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Models;

namespace WebApi.InputModels {
    public class ReminderValidator : AbstractValidator<ReminderInputModel>
    {
        public ReminderValidator()
        {
            RuleFor(m => m.Time).Must(BeAValidDate).WithMessage("Reminder time is required");
            RuleFor(m => m.TimeOffset).NotEmpty().WithMessage("Reminer time offset is required");
            RuleFor(m => m.TimeOffset).Custom((timeOffset, context) => {
                  if ((int)timeOffset < 1 || (int)timeOffset > 6) {
                    context.AddFailure("Reminder offset is not valid");
                }
            });
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}