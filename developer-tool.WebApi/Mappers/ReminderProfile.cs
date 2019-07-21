using AutoMapper;
using WebApi.ViewModels;
using WebApi.InputModels;
using Domain.PersistenceModels;
using Domain.Models;

public class ReminderProfile : Profile
{
	public ReminderProfile()
	{
		CreateMap<Reminder, ReminderViewModel>();
		CreateMap<ReminderInputModel, Reminder>()
			.ForMember(x => x.Id, opt => opt.Ignore());
		CreateMap<Reminder, ReminderModel>()
			.ForMember(x => x.Id, opt => opt.Ignore());
		CreateMap<ReminderModel, Reminder>();
	}
}