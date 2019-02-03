using AutoMapper;
using Infrastructure.Models;
using Domain.Models;
using WebApi.ViewModels;
using WebApi.InputModels;

public class ReminderProfile : Profile
{
	public ReminderProfile()
	{
		CreateMap<Domain.Models.Reminder, ReminderViewModel>();
		CreateMap<ReminderInputModel, Domain.Models.Reminder>()
			.ForMember(x => x.Id, opt => opt.Ignore());
		CreateMap<Domain.Models.Reminder, Infrastructure.Models.ReminderModel>()
			.ForMember(x => x.Id, opt => opt.Ignore());
		CreateMap<Infrastructure.Models.ReminderModel, Domain.Models.Reminder>();
	}
}