using FluentValidation;
using System.Linq;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Models;
using WebApi.InputModels;

namespace WebApi.ViewModels {
    public class CalendarEventValidator : AbstractValidator<CalendarEventInputModel>
    {
        public CalendarEventValidator(IRepository<Event> testRepository)
        {
            RuleFor(m => m.Title).NotEmpty();
            RuleFor(m => m.Title).MaximumLength(50);
            // RuleFor(m => m.Title)
            //     .Must((rootObject, property, context) => {
            //         if (rootObject.Id != 0 && testRepository.FindAll().Any(o => o.Title == property && o.Id != rootObject.Id))
            //             return false;

            //         if (rootObject.Id == 0 && testRepository.FindAll().Any(o => o.Title == property))
            //             return false;

            //         return true;
            //     })
            //     .WithMessage("Record already exists");
        }
    }
}