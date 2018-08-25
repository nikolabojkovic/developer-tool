using FluentValidation;
using System.Linq;
using WebApi.Interfaces;
using WebApi.Domain;
using WebApi.Models;
namespace WebApi.ViewModels {
    public class CreateCustomerValidator : AbstractValidator<TestViewModel>
    {
        public CreateCustomerValidator(IRepository<Test> testRepository)
        {
            RuleFor(m => m.FirstName)
                                     .Must((rootObject, property, context) => {
                                         if (rootObject.Id != 0 && testRepository.FindAll().Any(o => o.FirstName == property && o.Id != rootObject.Id))
                                            return false;

                                         if (rootObject.Id == 0 && testRepository.FindAll().Any(o => o.FirstName == property))
                                            return false;

                                         return true;
                                     })
                                     .WithMessage("Record already exists");
        }
    }
}