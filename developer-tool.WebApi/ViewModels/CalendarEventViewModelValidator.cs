using FluentValidation;
using System.Linq;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Models;
namespace WebApi.ViewModels {
    public class CreateCustomerValidator : AbstractValidator<CalendarEventViewModel>
    {
        public CreateCustomerValidator(IRepository<Event> testRepository)
        {
            RuleFor(m => m.Title)
                .Must((rootObject, property, context) => {
                    if (rootObject.Id != 0 && testRepository.FindAll().Any(o => o.Title == property && o.Id != rootObject.Id))
                        return false;

                    if (rootObject.Id == 0 && testRepository.FindAll().Any(o => o.Title == property))
                        return false;

                    return true;
                })
                .WithMessage("Record already exists");
        }
    }
}